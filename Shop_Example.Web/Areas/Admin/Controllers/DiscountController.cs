using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Admin.Discount;
using Shop_Example.Entities.Products;
using Shop_Example.Tools.TimeAndDate;
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
        private readonly Time _time;
        private readonly IUnitOfWork _unitOfWork;
        public DiscountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _time = new Time();
        }
        public async Task<IActionResult> Index()
        {
            var discounts = await _unitOfWork.DiscountRepository.GetAllAsync();

            var model = discounts.Select(p => new DiscountDto
            {
                Id = p.Id,
                CouponCode = p.CouponCode,
                Name = p.Name,
                StartDate = p.StartDate != null ? _time.ToShamsi((DateTime)p.StartDate) : "تعیین نشده",
                EndDate = p.EndDate != null ? _time.ToShamsi((DateTime)p.EndDate) : "تعیین نشده",
                ValueDiscount = p.UsePercentage ? p.DiscountPercentage.ToString() + " %" : p.DiscountAmount.ToString() + " تومان"
            }).ToList();

            return View(model);
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

            return RedirectToAction("Index", "Discount");
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

        [HttpDelete]
        [Route("/Discount/Delete/{DiscountId}")]
        public async Task<bool> Delete(long DiscountId)
        {
            var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(DiscountId);

            if (discount != null)
            {
                return await _unitOfWork.DiscountRepository.RemoveAsync(discount);
            }

            return false;
        }

        public async Task<IActionResult> Detail(long DiscountId)
        {
            var discount = _unitOfWork.DiscountRepository.GetAllAsync(p => p.Id == DiscountId
                                                                            , include => include.Products).Result.FirstOrDefault();

            if (discount == null)
            {
                return BadRequest();
            }

            DetailDiscountDto model = new DetailDiscountDto
            {
                Id = discount.Id,
                Name = discount.Name,
                CouponCode = discount.CouponCode,
                ValueDiscount = discount.UsePercentage ? discount.DiscountPercentage.ToString() + " %" : discount.DiscountAmount.ToString() + " تومان",
                StartDate = discount.StartDate != null ? _time.ToShamsi((DateTime)discount.StartDate) : "تعیین نشده",
                EndDate = discount.EndDate != null ? _time.ToShamsi((DateTime)discount.EndDate) : "تعیین نشده",
                DiscountType = discount.DiscountType == Entities.Products.DiscountType.AssignedProduct ? "برای محصولات" : "برای دسته بندی ها",
                Limitation = discount.DiscountLimitation == Entities.Products.DiscountLimitationType.Unlimited ? "نا محدود"
                            : discount.LimitationTimes.ToString() + " بار",
                RequiresCouponCode = discount.RequiresCouponCode
            };

            if (discount.Products != null)
            {
                model.Products = discount.Products.Select(p => new ProductDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name
                }).ToList();
            }

            return View(model);
        }
    }

}
