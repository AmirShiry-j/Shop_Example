using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Example.Entities.Billboard;

namespace Shop_Example.Web.Component
{
    public class ShowRow2BillboardComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShowRow2BillboardComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = await _unitOfWork.SliderRepository.GetAllAsync(p => p.Displayed &&
                                                                        p.Location == SliderLocation.Row2);

            return View("/Views/Component/ShowRow2Billboard.cshtml", sliders);
        }
    }
}
