using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Component
{
    public class ShowDetailsProductInModalComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShowDetailsProductInModalComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync(int ProductId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);

            List<string> ImagesNames = _unitOfWork.ProductImagesRepository.GetAllAsync(p => p.ProductId == product.Id).Result.Select(p => p.Image).ToList();
            
            var tuple = new Tuple<Product, List<string>>(product, ImagesNames);

            return View("/Views/Component/ShowDetailsProductInModal.cshtml", tuple);
        }
    }
}
