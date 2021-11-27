using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Entities.Billboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.ViewComponents.Home
{
    [ViewComponent]
    public class ShowBottomBillboards : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShowBottomBillboards(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = await _unitOfWork.SliderRepository.GetAllAsync(p => p.Displayed
                                                                        && (p.Location == SliderLocation.Bottom));

            return View("/Views/Home/ViewComponets/ShowBottomBillboards.cshtml", sliders);
        }
    }
}
