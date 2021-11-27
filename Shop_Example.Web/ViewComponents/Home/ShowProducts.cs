using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Product;
using Shop_Example.Entities.Models;
using Shop_Example.Web.Tools.DiscountHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Component
{
    [ViewComponent]
    public class ShowProducts : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Discount _discount;
        public ShowProducts(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _discount = new Discount();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var categories = await _unitOfWork.HomeCategoryRepository
                .GetAllAsync(null, include => include.Category,
                                   include => include.Category.Products);

            List<CategoryWithHisProductsDto> model = categories.Select(c => new CategoryWithHisProductsDto
            {
                Id = c.CategoryId,
                Name = c.Category.Name,
                Products = c.Category.Products.Where(p => p.Displayed).Select(p => new ShortInfoProductDto()
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    Image = p.Image,
                    ShowedPrice = _discount.GetShowedPrice(p.Price, p.Discount),
                    LinedPrice = _discount.GetLinedPrice(p.Price, p.Discount)

                }).ToList()
            }).ToList();

            return View("/Views/Home/ViewComponets/ShowProducts.cshtml", model);
        }

    }
}
