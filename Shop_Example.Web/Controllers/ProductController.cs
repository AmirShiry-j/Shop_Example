using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Product;
using Shop_Example.Entities.Models;
using Shop_Example.Entities.Products.Comments;
using Shop_Example.Web.Tools.DiscountHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Discount _discount;

        private readonly UserManager<User> _userManager;
        public ProductController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _discount = new Discount();

            _userManager = userManager;
        }
        public async Task<IActionResult> Detail(int ProductId)
        {

            var product = _unitOfWork.ProductRepository.GetAllAsync(p => p.Id == ProductId && p.Displayed == true,
                                                        p => p.ProductImages,
                                                        p => p.ProductFeatures,
                                                        p => p.ProductTages,
                                                        p => p.Categories,
                                                        p => p.Warranty).Result.FirstOrDefault();

            if (product == null)
            {
                return RedirectToRoute("Error", "Home");
            }

            decimal avgStars = 1;//پیش فرض 

            product.Comments = _unitOfWork.CommentRepository.GetAllAsync(p => p.ProductId == product.Id, p => p.Stars).Result.ToList();

            if (product.Comments != null)
            {
                //میانگین تعداد رای ها به محصول از نظرات کاربران
                avgStars = product.Comments.Select(p => p.Stars).ToList().Average(p => p.AverageStars);

            }

            var model = new DatailsProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Image = product.Image,
                Model = product.Model,
                ShowedPrice = _discount.GetShowedPrice(product.Price, product.Discount),
                LinedPrice = _discount.GetLinedPrice(product.Price, product.Discount),
                Warranty = product.Warranty != null ? new WarrantyDto { Id = product.Warranty.Id, Name = product.Warranty.Name } : null,
                Tages = product.ProductTages.Select(t => t.Value).ToList(),
                ProductFeatures = product.ProductFeatures.Select(f => new FeatureDto { DisplayName = f.DisplayName, Value = f.Value }).ToList(),
                SrcImages = product.ProductImages.Select(m => m.Image).ToList(),
                Categories = product.Categories.Select(c => new CategoryDto { Id = c.CategoryId, Name = c.Name }).ToList(),

                ProductStarts = Convert.ToByte(avgStars),
            };

            return View(model);
        }
    }
}
