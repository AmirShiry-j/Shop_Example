using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Entities.Models;
using Shop_Example.Web.Tools.CheckImageValidation;
using Microsoft.AspNetCore.Authorization;
using Shop_Example.Dtoes.Admin.Product;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _context;

        public ProductController(IUnitOfWork context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int Page = 1)
        {
            var products = await _context.ProductRepository.GetAllAsync(null, p => p.Warranty);

            int countInPage = 10;

            var model = new ProductListVM
            {
                Page = Page,
                CountAllItems = products.Count(),
                CounInPage = countInPage
            };

            model.Products = products.Skip((Page - 1) * model.CounInPage).Take(model.CounInPage)
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Displayed = p.Displayed,
                    Id = p.Id,
                    Image = p.Image,
                    Price = p.Price,
                    HasWarranty = p.Warranty != null ? true : false
                }).ToList();

            return View(model);
        }
        

        [Route("/Admin/P/Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categorys = await _context.CategoryRepository.GetAllAsync();
            return View();
        }

        [ValidateAntiForgeryToken]
        [Route("/Admin/P/Create")]
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile ImgUp, AddNewProductDto viewModel, List<int> selected)
        {
            ViewBag.SelectedList = selected;
            ViewBag.Categorys = await _context.CategoryRepository.GetAllAsync();
            if (selected.Count > 0)
            {
                if (ImgUp != null && ValidationImage.Validate(ImgUp))
                {
                    if (ModelState.IsValid)
                    {
                        Product newProduct = new Product()
                        {
                            Name = viewModel.Name,
                            Price = viewModel.Price,
                            Displayed = viewModel.Displayed,
                            Brand = viewModel.Brand,
                            Description = viewModel.Description,
                            Model = viewModel.Model,
                            Count = viewModel.Count,
                            Discount = viewModel.Discount,
                            Views = 0,
                            Categories = new List<Category>()
                        };

                        newProduct.Image = Guid.NewGuid() + Path.GetExtension(ImgUp.FileName);

                        string path = Path.Combine(Directory.GetCurrentDirectory(), "Images/ProductImages", newProduct.Image);

                        using (FileStream fs = System.IO.File.Create(path))
                        {
                            await ImgUp.CopyToAsync(fs);
                        }

                        if (await _context.ProductRepository.AddAsync(newProduct))
                        {
                            foreach (int item in selected)
                            {
                                var category = await _context.CategoryRepository.GetByIdAsync(item);
                                newProduct.Categories.Add(category);
                            }

                            await _context.ProductRepository.UpdateAsync(newProduct);

                            foreach (var item in viewModel.Tages.Split("-"))
                            {
                                await _context.TagesRepository.AddAsync(new ProductTages()
                                {
                                    Value = item,
                                    ProductId = newProduct.Id
                                });
                            }

                            return Redirect("/Admin/Product");
                        }
                        else
                        {
                            return View(viewModel);
                        }
                    }
                    else
                    {
                        return View(viewModel);
                    }
                }
                else
                {
                    ViewBag.ImageError = "تصویری وارد نشده یا فرمت وارد شده اشتباه است";
                    return View(viewModel);
                }
            }
            else
            {
                ViewBag.SelectedErrore = "گروه های انتخابی نمیاوتنند خالی باشند";
                return View(viewModel);
            }
        }

        [Route("/Admin/P/Edit/{Id}")]

        public async Task<IActionResult> Edit(int id)
        {
            Product product = _context.ProductRepository.GetAllAsync(p => p.Id == id, p => p.Categories).Result.FirstOrDefault();

            if (product != null)
            {
                ViewBag.Categorys = await _context.CategoryRepository.GetAllAsync();

                ViewBag.SelectedList = product.Categories
                    .Select(c => c.CategoryId)
                    .ToList();

                string tags = string.Join('-', _context.TagesRepository
                    .GetAllAsync(c => c.ProductId == product.Id)
                    .Result
                    .Select(c => c.Value)
                    .ToList());

                var model = new EditProductDto
                {
                    Id = product.Id,
                    Price = product.Price,
                    Brand = product.Brand,
                    Displayed = product.Displayed,
                    Description = product.Description,
                    Name = product.Name,
                    Count = product.Count,
                    Model = product.Model,
                    Discount = product.Discount,
                    Image = product.Image,
                    Tages = tags
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }


        [ValidateAntiForgeryToken]
        [Route("/Admin/P/Edit/{Id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile ImgUp, EditProductDto viewModel, List<int> selected)
        {
            ViewBag.SelectedList = selected;
            ViewBag.Categorys = await _context.CategoryRepository.GetAllAsync();

            if (selected.Count > 0)
            {
                if (ModelState.IsValid)
                {
                    Product product = _context.ProductRepository.GetAllAsync(p => p.Id == viewModel.Id, p => p.Categories).Result.FirstOrDefault();

                    if (product == null)
                    {
                        return NotFound();
                    }

                    if (ImgUp != null && ValidationImage.Validate(ImgUp))
                    {
                        System.IO.File.Delete(Path.Combine(
                            Directory.GetCurrentDirectory(), "Images/ProductImages", viewModel.Image
                        ));

                        viewModel.Image = Guid.NewGuid() + Path.GetExtension(ImgUp.FileName);

                        string path = Path.Combine(Directory.GetCurrentDirectory(), "Images/ProductImages", viewModel.Image);

                        using (FileStream fs = System.IO.File.Create(path))
                        {
                            await ImgUp.CopyToAsync(fs);
                        }
                    }

                    product.Brand = viewModel.Brand;
                    product.Model = viewModel.Model;
                    product.Description = viewModel.Description;
                    product.Displayed = viewModel.Displayed;
                    product.Image = viewModel.Image;
                    product.Count = viewModel.Count;
                    product.Name = viewModel.Name;
                    product.Price = viewModel.Price;
                    product.Discount = viewModel.Discount;

                    if (await _context.ProductRepository.UpdateAsync(product))
                    {

                        product.Categories.Clear();

                        foreach (int item in selected)
                        {
                            var category = await _context.CategoryRepository.GetByIdAsync(item);
                            product.Categories.Add(category);
                        }

                        await _context.ProductRepository.UpdateAsync(product);

                        await _context.TagesRepository
                            .RemoveReangeAsync(
                                _context.TagesRepository
                                    .GetAllAsync(c => c.ProductId == product.Id)
                                    .Result
                                    .ToList()
                            );

                        foreach (var item in viewModel.Tages.Split("-"))
                        {
                            await _context.TagesRepository.AddAsync(new ProductTages()
                            {
                                Value = item,
                                ProductId = viewModel.Id
                            });
                        }

                        return Redirect("/Admin/Product");
                    }
                    else
                    {
                        return View(viewModel);
                    }
                }

                return View(viewModel);

            }

            ViewBag.SelectedErrore = "گروه های انتخابی نمیاوتنند خالی باشند";
            return View(viewModel);
        }

        [Route("/Admin/P/Delete/{Id}")]
        public async Task Delete(int id)
        {
            Product product = await _context.ProductRepository
                .GetByIdAsync(id);

            if (product != null)
            {
                _context.TagesRepository
                    .GetAllAsync(t => t.ProductId == product.Id)
                    .Result
                    .ToList()
                    .ForEach(tg => _context.TagesRepository.RemoveAsync(tg));

                var images = await _context.ProductImagesRepository
                    .GetAllAsync(p => p.ProductId == product.Id);
                if (images != null)
                {
                    foreach (var item in images)
                    {
                        System.IO.File.Delete(Path.Combine(
                            Directory.GetCurrentDirectory(), "Images/ProductImages", item.Image
                        ));

                        await _context.ProductImagesRepository.RemoveAsync(item);
                    }
                }

                System.IO.File.Delete(Path.Combine(
                    Directory.GetCurrentDirectory(), "Images/ProductImages", product.Image
                    ));

                await _context.ProductRepository.RemoveAsync(product);

            }
        }
    }
}
