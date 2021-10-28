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
    public class ShowProductsComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Discount _discount;
        public ShowProductsComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _discount = new Discount();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var categories = _unitOfWork.CategoryRepository
                .GetAllAsync(p => p.Products.Count(p => p.Displayed == true) > 0
                , include => include.Products).Result;

            List<CategoryWithHisProductsDto> model = categories.Select(c => new CategoryWithHisProductsDto
            {
                Id = c.CategoryId,
                Name = c.Name,
                Products = c.Products.Select(p => new ShortInfoProductDto()
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    Image = p.Image,
                    ShowedPrice = _discount.GetShowedPrice(p.Price, p.Discount),
                    LinedPrice = _discount.GetLinedPrice(p.Price, p.Discount)

                }).ToList()
            }).ToList();

            return View("/Views/Component/ShowProducts.cshtml", model);
        }

    }
}
