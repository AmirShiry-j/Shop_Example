using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Product;
using Shop_Example.Entities.Billboard;
using Shop_Example.Web.Tools.DiscountHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.ViewComponents.Home
{
    [ViewComponent]
    public class ShowMomentSliders : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Discount _discount;
        public ShowMomentSliders(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _discount = new Discount();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lastProducts = _unitOfWork.ProductRepository.GetAllAsync(p => p.Displayed)
                .Result
                .TakeLast(5);

            List<ShortInfoProductDto> model = lastProducts.Select(p => new ShortInfoProductDto
            {
                ProductId = p.Id,
                Image = p.Image,
                Name = p.Name,
                LinedPrice = _discount.GetLinedPrice(p.Price, p.Discount),
                ShowedPrice = _discount.GetShowedPrice(p.Price, p.Discount)
            }).ToList();

            return View("/Views/Home/ViewComponets/ShowMomentSliders.cshtml", model);
        }
    }
}
