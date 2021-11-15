using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Admin.Discount;
using Shop_Example.Entities.Products;
using Shop_Example.Web.Areas.Admin.ModelBinders.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class DiscountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DiscountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([ModelBinder(binderType: typeof(DiscountEntityBinder))] CreateDiscountDto model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            var newDiscount = new Discount
            {
                Name = model.Name,
                CouponCode = model.CouponCode,
                DiscountAmount = model.DiscountAmount,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                UsePercentage = model.UsePercentage,
                RequiresCouponCode = model.RequiresCouponCode,
                DiscountLimitation = (Shop_Example.Entities.Products.DiscountLimitationType)model.DiscountLimitation,
                DiscountPercentage = model.DiscountPercentage,
                DiscountType = (Shop_Example.Entities.Products.DiscountType)model.DiscountType,
                LimitationTimes = model.LimitationTimes,
            };
            if (model.ApplidToProductItem != null)
            {
                var products = await _unitOfWork.ProductRepository
                    .GetAllAsync(p => model.ApplidToProductItem.Any(a => p.Id == a));

                newDiscount.Products = products.ToList();
            }

            var resultCreateDiscount = await _unitOfWork.DiscountRepository.AddAsync(newDiscount);

            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> SearchProductItem(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var products = _unitOfWork.ProductRepository
                    .GetAllAsync(p => p.Name.Contains(term)).Result
                    .Select(p => new ProductItemDto
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList();

                return Ok(products);
            }
            else
            {
                var products = _unitOfWork.ProductRepository
                    .GetAllAsync().Result
                    .OrderByDescending(p => p.Id).Take(10)
                    .Select(p => new ProductItemDto
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList();

                return Ok(products);

            }
        }
    }
}
