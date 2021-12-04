using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Admin.HomeCategory;
using Shop_Example.Entities.Home.HomeCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    [Route("/{Controller}/{Action}/")]
    public class HomeCategoryController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public HomeCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var model = _unitOfWork.HomeCategoryRepository.GetAllAsync(null, include => include.Category).Result
                .Select(p => new HomeCategoryDto { Id = p.Id, CategoryId = p.CategoryId, CategoryName = p.Category.Name });

            return View(model);
        }


        public async Task<IActionResult> Create()
        {
            List<SelectListItem> Categories = new List<SelectListItem>
            {
                new SelectListItem{Disabled=true, Selected=true,Text="لطفا دسته بندی مورد نظرتون رو انتخاب کنید", Value="0"}
            };
            Categories.AddRange(_unitOfWork.CategoryRepository.GetAllAsync().Result.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.CategoryId.ToString()

            }));
            var model = new CreateHomeCategoryDto
            {
                Categories = Categories
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHomeCategoryDto model)
        {
            if (ModelState.IsValid == false)
            {
                List<SelectListItem> Categories = new List<SelectListItem>
                {
                    new SelectListItem{Disabled=true, Selected=true,Text="لطفا دسته بندی مورد نظرتون رو انتخاب کنید", Value="0"}
                };
                Categories.AddRange(_unitOfWork.CategoryRepository.GetAllAsync().Result.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.CategoryId.ToString()

                }));
                model.Categories = Categories;

                return View(model);
            }

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(model.CategoryId);

            if (category == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (_unitOfWork.HomeCategoryRepository.GetAllAsync(p => p.CategoryId == category.CategoryId).Result.Any())
            {
                List<SelectListItem> Categories = new List<SelectListItem>
                {
                    new SelectListItem{Disabled=true, Selected=true,Text="لطفا دسته بندی مورد نظرتون رو انتخاب کنید", Value="0"}
                };
                Categories.AddRange(_unitOfWork.CategoryRepository.GetAllAsync().Result.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.CategoryId.ToString()

                }));
                model.Categories = Categories;

                ModelState.AddModelError("", "این دسته بندی قبلا اضافه شده");

                return View(model);
            }


            //Add new HomeCategory

            bool result = await _unitOfWork.HomeCategoryRepository.AddAsync(new HomeCategory
            {
                CategoryId = category.CategoryId
            });

            return RedirectToAction("Index", "HomeCategory");
        }

        [HttpDelete("{HomeCategoryId}")]
        public async Task<bool> Delete(int HomeCategoryId)
        {
            var homeCategory = await _unitOfWork.HomeCategoryRepository.GetByIdAsync(HomeCategoryId);

            if (homeCategory == null)
            {
                return false;
            }

            var resultDelete = await _unitOfWork.HomeCategoryRepository.RemoveAsync(homeCategory);

            return resultDelete;
        }
    }
}
