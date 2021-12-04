using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Entities.Models;
using Shop_Example.Web.Tools.CheckImageValidation;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Area("Admin,Manager")]
    public class ProductImagesController : Controller
    {
        private IUnitOfWork _repository;

        public ProductImagesController(IUnitOfWork repository)
        {
            _repository = repository;
        }

        [Route("/Admin/PI/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            Product product = await _repository.ProductRepository.GetByIdAsync(id);
            if (product != null)
            {
                return View(id);
            }

            return NotFound();
        }


        [Route("/Admin/Pi/ShowTable/{id}")]
        public async Task<IActionResult> ShowTable(int id)
        {
            var productImage = await _repository.ProductImagesRepository
                .GetAllAsync(p => p.ProductId == id);
            if (productImage != null)
            {
                return PartialView(productImage);
            }

            return Content("چیزی نیست بمولا");
        }


        [Route("/Admin/Pi/Add/{id}")]
        public async Task<IActionResult> Add(int id)
        {
            if (await _repository.ProductRepository.GetByIdAsync(id) != null)
            {
                return PartialView(new ProductImages()
                {
                    ProductId = id
                });
            }

            return Conflict("خیخیخیخیخیخیخی");
        }


        [Route("/Admin/Pi/Add/{id}")]
        [HttpPost]
        public async Task<IActionResult> Add(ProductImages images,IFormFile ImgUp)
        {
            if (ImgUp != null)
            {
                if (ValidationImage.Validate(ImgUp))
                {
                    images.Image = Guid.NewGuid().ToString() + Path.GetExtension(ImgUp.FileName);
                    using (FileStream fs = System.IO.File.Create(
                        Path.Combine(Directory.GetCurrentDirectory(),"Images/ProductImages/",images.Image)
                        ))
                    {
                        await ImgUp.CopyToAsync(fs);
                    }

                    await _repository.ProductImagesRepository.AddAsync(images);
                }

            }

            return null;
        }



        [Route("/Admin/Pi/Delete/{id}")]
        public async Task Delete(int id)
        {
            ProductImages images = await _repository.ProductImagesRepository.GetByIdAsync(id);
            if (images != null)
            {
                System.IO.File.Delete(Path.Combine(
                    Directory.GetCurrentDirectory(), "Images/ProductImages/", images.Image
                ));

                await _repository.ProductImagesRepository.RemoveAsync(images);
            }
        }
    }
}
