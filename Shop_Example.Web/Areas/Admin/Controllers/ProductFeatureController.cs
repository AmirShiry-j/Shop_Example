using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Admin.ProductFeature;
using Shop_Example.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,Manager")]
    public class ProductFeatureController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductFeatureController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(int ProductId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);

            if (product == null)
                return BadRequest();

            var featuresProduct = _unitOfWork.ProductFeatureRepository.GetAllAsync(p => p.ProductId == product.Id).Result
                .Select(p => new ListProductFeatureDto { Id = p.Id, DisplayName = p.DisplayName, Value = p.Value }).ToList();


            var tuple = new Tuple<string, IEnumerable<ListProductFeatureDto>>(product.Name, featuresProduct);
            ViewBag.ProductId = ProductId;

            return View(tuple);
        }

        public async Task<IActionResult> Create(int ProductId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);

            if (product == null)
                return BadRequest();

            var model = new AddFeatureInProductDto
            {
                ProductId = ProductId,
                ProductName = product.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddFeatureInProductDto model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(model.ProductId);
            if (product == null)
                return BadRequest();

            var productFeature = new ProductFeature
            {
                DisplayName = model.DisplayName,
                Value = model.Value,
                Product = product
            };

            var resultAddFeature = await _unitOfWork.ProductFeatureRepository.AddAsync(productFeature);
            if (resultAddFeature)
            {
                TempData["Message"] = "ویژگی جدید با موفقیت به محصول اضافه شد";

                return RedirectToAction("Create", new { ProductId = product.Id });
            }
            else
            {
                ViewBag.Message = "ویژگی مورد نظر به محصول اضافه نشد";
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(long FeatureId)
        {
            var feature = await _unitOfWork.ProductFeatureRepository.GetByIdAsync(FeatureId);

            if (feature == null)
                return BadRequest();

            var resultDeleteFeature = await _unitOfWork.ProductFeatureRepository.RemoveAsync(feature);

            return RedirectToAction("Index", new { feature.ProductId });
        }

        public async Task<IActionResult> Edit(long FeatureId)
        {
            var feature = await _unitOfWork.ProductFeatureRepository.GetByIdAsync(FeatureId);

            if (feature == null)
                return BadRequest();

            var model = new EditFeatureInProductDto
            {
                ProductId = feature.ProductId,
                DisplayName = feature.DisplayName,
                Id=feature.Id,
                Value = feature.Value,
                ProductName = _unitOfWork.ProductRepository.GetByIdAsync(feature.ProductId).Result.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditFeatureInProductDto model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            var feature = await _unitOfWork.ProductFeatureRepository.GetByIdAsync(model.Id);
            if (feature == null)
                return BadRequest();

            feature.DisplayName = model.DisplayName;
            feature.Value = model.Value;

            var resultUpdate = await _unitOfWork.ProductFeatureRepository.UpdateAsync(feature);

            return RedirectToAction("Index", new { feature.ProductId });
        }
    }
}
