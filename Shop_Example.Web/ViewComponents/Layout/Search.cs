using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Component.Layout
{
    [ViewComponent]
    public class Search : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public Search(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            var model = categories.Select(p => new CategoryDto { Id = p.CategoryId, Name = p.Name });

            return View("/Views/Shared/ViewComponents/Search/Search.cshtml", model);
        }

    }
}
