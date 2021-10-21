using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Admin.Warranty;
using Shop_Example.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class WarrantyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public WarrantyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(int ProductId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);
            if (product == null)
                return NotFound("محصولی با این آیدی موجود نیست");

            var warranty = _unitOfWork.WarrantyRepository.GetAllAsync(p => p.ProductId == product.Id).Result.FirstOrDefault();

            ViewBag.ProductName = product.Name;
            ViewBag.ProductId = product.Id;

            return View(warranty);
        }
        public async Task<IActionResult> Create(int ProductId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);
            if (product == null)
                return NotFound("محصولی با این آیدی موجود نیست");

            var warranty = _unitOfWork.WarrantyRepository.GetAllAsync(p => p.ProductId == product.Id).Result.FirstOrDefault();

            if (warranty != null)
            {
                RedirectToAction("Edit", new { WarrantyId = warranty.Id });
            }

            var model = new CreateWarrantyDto
            {
                ProductId = product.Id
            };

            ViewBag.ProductName = product.Name;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWarrantyDto model)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(model.ProductId);

            if (product == null)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var newWarranty = new Warranty
                {
                    Name = model.Name,
                    Description = model.Description,
                    ProductId = product.Id
                };

                if (await _unitOfWork.WarrantyRepository.AddAsync(newWarranty))
                {
                    return RedirectToAction(nameof(Index), new { ProductId = product.Id });
                }
                else
                {
                    ModelState.AddModelError("", "گارانتی به محصول اضافه نشد");

                    return View(model);
                }
            }
            else
                return View(model);
        }

        public async Task<IActionResult> Edit(int WarrantyId)
        {
            var warranty = await _unitOfWork.WarrantyRepository.GetByIdAsync(WarrantyId);

            if (warranty == null)
                return NotFound("گارانتی با این آیدی یافت نشد");

            var model = new EditWarrantyDto
            {
                WarrantyId = warranty.Id,
                Description = warranty.Description,
                Name = warranty.Name,
                ProductId = warranty.ProductId
            };

            var product = _unitOfWork.ProductRepository.GetAllAsync(p => p.Id == warranty.ProductId).Result.FirstOrDefault();
            ViewBag.ProductName = product.Name;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditWarrantyDto model)
        {
            if (ModelState.IsValid)
            {
                var editWarranty = await _unitOfWork.WarrantyRepository.GetByIdAsync(model.WarrantyId);

                if (editWarranty == null)
                    return BadRequest();

                editWarranty.Name = model.Name;
                editWarranty.Description = model.Description;

                if (await _unitOfWork.WarrantyRepository.UpdateAsync(editWarranty))
                {
                    return RedirectToAction(nameof(Index), new { editWarranty.ProductId });
                }
                else
                {
                    ModelState.AddModelError("", "گارانتی ادیت نشد");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
        public async Task<bool> Delete(int WarrantyId)
        {
            var warranty = await _unitOfWork.WarrantyRepository.GetByIdAsync(WarrantyId);

            if (warranty == null)
                return false;

            return await _unitOfWork.WarrantyRepository.RemoveAsync(warranty);
        }
    }
}
