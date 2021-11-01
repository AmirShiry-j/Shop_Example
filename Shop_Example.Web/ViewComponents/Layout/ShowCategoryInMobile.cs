using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Component
{
    [ViewComponent]
    public class ShowCategoryInMobile:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShowCategoryInMobile(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            return View("/Views/Shared/ViewComponents/CategoryMenus/ShowCategoryInMobile.cshtml", categories);
        }
    }
}
