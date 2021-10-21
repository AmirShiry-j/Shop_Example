using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Admin.Slider;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using Shop_Example.Web.Tools.CheckImageValidation;
using Shop_Example.Entities.Billboard;
using Microsoft.AspNetCore.Authorization;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SliderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _unitOfWork.SliderRepository.GetAllAsync();

            return View(sliders);
        }

        public async Task<IActionResult> Create()
        {

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderDto model, IFormFile image)
        {
            if (image != null && ValidationImage.Validate(image))
            {
                if (ModelState.IsValid)
                {
                    string nameImg = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    string path = Path.Combine("Images/SliderImages/", nameImg);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var newSlider = new Slider
                    {
                        Displayed = model.Displayed,
                        Link = model.Link,
                        Src = nameImg,
                        Location=model.Location
                    };

                    if (await _unitOfWork.SliderRepository.AddAsync(newSlider))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        ModelState.AddModelError("", "اسلایدر ایجاد نشد");
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                ViewBag.ErrorMessage = "لطفا یک تصویر برای اسلایدر خود انتخاب کنید";
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var slider = await _unitOfWork.SliderRepository.GetByIdAsync(Id);

            if (slider != null)
            {
                var model = new EditSliderDto
                {
                    Id = slider.Id,
                    Displayed = slider.Displayed,
                    Link = slider.Link,
                    Src=slider.Src,
                    Location=slider.Location
                };

                return View(model);
            }
            else
                return NotFound("اسلاید یافت نشد");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditSliderDto model, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var editSlide = await _unitOfWork.SliderRepository.GetByIdAsync(model.Id);

                if (editSlide == null)
                    return BadRequest();

                if (image != null && ValidationImage.Validate(image))
                {
                    string oldPath = "Images/SliderImages/" + model.Src;
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);

                    editSlide.Src = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    string newPath = "Images/SliderImages/" + editSlide.Src;

                    using (var stream=new FileStream(newPath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                }

                editSlide.Displayed = model.Displayed;
                editSlide.Location = model.Location;
                editSlide.Link = model.Link;

                if (await _unitOfWork.SliderRepository.UpdateAsync(editSlide))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "اسلایدر به دلیل برخی مشکلات ویرایش نشد");

                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<bool> Delete(int Id)
        {
            var slider = await _unitOfWork.SliderRepository.GetByIdAsync(Id);

            if (slider != null)
            {
                string path = "Images/SliderImages/" + slider.Src;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                if (await _unitOfWork.SliderRepository.RemoveAsync(slider))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
