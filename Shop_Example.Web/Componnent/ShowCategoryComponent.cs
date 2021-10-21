using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;

namespace Shop_Example.Web.Componnent
{
    public class ShowCategoryComponent : ViewComponent
    {
        private IUnitOfWork _repository;


        public ShowCategoryComponent(IUnitOfWork repository)
        {
            _repository = repository;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {       

            var model = await _repository.CategoryRepository.GetAllAsync();
            return View("/Views/Component/ShowCategory.cshtml", model);
        }
    }
}
