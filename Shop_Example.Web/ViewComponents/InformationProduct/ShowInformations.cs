using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.ViewComponents.InformationProduct
{
    [ViewComponent]
    public class ShowInformations : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShowInformations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<IViewComponentResult> InvokeAsync(int ProductId)
        {
            var product = _unitOfWork.ProductRepository.GetAllAsync(p => p.Id == ProductId,
                                                                            includ => includ.ProductInformation).Result.FirstOrDefault();


            InformationVM model = new InformationVM()
            {
                ProductModelName = product.Model
            };

            if (product.ProductInformation != null && product.ProductInformation.Any())
            {
                model.Information = product.ProductInformation.Select(p => new InformationDto
                {
                    DisplayName = p.DisplayName,
                    Value = p.Value
                }).ToList();

            }

            return View("/Views/Product/ViewComponents/ShowInformation.cshtml", model);
        }
    }
}
