using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Admin.ProductInformation;
using Shop_Example.Entities.Models;
using Shop_Example.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductInformationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductInformationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(int ProductId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);

            if (product == null)
                return BadRequest();

            var infoProduct = _unitOfWork.ProductInformationRepository.GetAllAsync(p => p.ProductId == product.Id).Result
                .Select(p => new ProductInformationDto { Id = p.Id, DisplayName = p.DisplayName, Value = p.Value }).ToList();


            var tuple = new Tuple<string, IEnumerable<ProductInformationDto>>(product.Name, infoProduct);
            ViewBag.ProductId = ProductId;

            return View(tuple);
        }

        public async Task<IActionResult> Create(int ProductId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);

            if (product == null)
                return BadRequest();

            var model = new AddInformationInProductDto
            {
                ProductId = ProductId,
                ProductName = product.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddInformationInProductDto model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(model.ProductId);
            if (product == null)
                return BadRequest();

            var productInformation = new ProductInformation
            {
                DisplayName = model.DisplayName,
                Value = model.Value,
                Product = product
            };

            var resultAddInformation = await _unitOfWork.ProductInformationRepository.AddAsync(productInformation);
            if (resultAddInformation)
            {
                TempData["Message"] = "اطلاعات جدید با موفقیت به محصول اضافه شد";

                return RedirectToAction("Create", new { ProductId = product.Id });
            }
            else
            {
                ViewBag.Message = "اطلاعات مورد نظر به محصول اضافه نشد";
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(long InformationId)
        {
            var information = await _unitOfWork.ProductInformationRepository.GetByIdAsync(InformationId);

            if (information == null)
                return BadRequest();

            var resultDeleteInformation = await _unitOfWork.ProductInformationRepository.RemoveAsync(information);

            return RedirectToAction("Index", new { information.ProductId });
        }

        public async Task<IActionResult> Edit(long InformationId)
        {
            var information = await _unitOfWork.ProductInformationRepository.GetByIdAsync(InformationId);

            if (information == null)
                return BadRequest();

            var model = new EditInformationInProductDto
            {
                ProductId = information.ProductId,
                DisplayName = information.DisplayName,
                Id = information.Id,
                Value = information.Value,
                ProductName = _unitOfWork.ProductRepository.GetByIdAsync(information.ProductId).Result.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditInformationInProductDto model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            var information = await _unitOfWork.ProductInformationRepository.GetByIdAsync(model.Id);
            if (information == null)
                return BadRequest();

            information.DisplayName = model.DisplayName;
            information.Value = model.Value;

            var resultUpdate = await _unitOfWork.ProductInformationRepository.UpdateAsync(information);

            return RedirectToAction("Index", new { information.ProductId });
        }
    }
}
