using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Componnent
{
    public class ShowCategoryInMobileComponent:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShowCategoryInMobileComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            return View("~/Views/Component/ShowCategoryInMobile.cshtml", categories);
        }
    }
}
