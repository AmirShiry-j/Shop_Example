using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;

namespace Shop_Example.Web.Component
{
    [ViewComponent]
    public class ShowCategory : ViewComponent
    {
        private IUnitOfWork _repository;


        public ShowCategory(IUnitOfWork repository)
        {
            _repository = repository;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {       

            var model = await _repository.CategoryRepository.GetAllAsync();
            return View("/Views/Shared/ViewComponents/CategoryMenus/ShowCategory.cshtml", model);
        }
    }
}
