using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Entities.Models;
using Shop_Example.Dtoes.Admin.Category;
using Microsoft.AspNetCore.Authorization;
using Shop_Example.Dtoes.Admin.Product;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _context;

        public CategoryController(IUnitOfWork context)
        {
            _context = context;
        }


        [Route("/Admin/Category")]
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        [Route("/Admin/C/ShowTable")]
        public async Task<PartialViewResult> ShowTable()
        {
            return PartialView(await _context.CategoryRepository.GetAllAsync());
        }


        [Route("/Admin/C/Create/{parentId?}")]
        public async Task<IActionResult> Create(int? parentId)
        {

            return PartialView(new AddNewCategoryDto()
            {
                ParentId = parentId
            });
        }


        [Route("/Admin/C/Create/{parentId?}")]
        [HttpPost]
        public async Task<IActionResult> Create(AddNewCategoryDto model)
        {
            if (model.Title != null)
            {
                Category category = new Category
                {
                    ParentId = model.ParentId,
                    Name = model.Title
                };
                await _context.CategoryRepository.AddAsync(category);

                return RedirectToAction("Index");

            }

            return View(model);
        }


        [Route("/Admin/c/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.CategoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                var model = new EditCategoryDto
                {
                    CategoryId = category.CategoryId,
                    ParentId = category.ParentId,
                    Title = category.Name
                };
                return PartialView(model);
            }
            else
            {
                return Content("گشتم نبود . نگرد نیست");
            }
        }



        [Route("/Admin/c/Edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryDto model)
        {
            if (ModelState.IsValid)
            {
                var category = await _context.CategoryRepository.GetByIdAsync(model.CategoryId);
                category.Name = model.Title;
                category.ParentId = model.ParentId;
                if (await _context.CategoryRepository.UpdateAsync(category))
                {
                    return null;
                }
                return null;
            }

            return null;
        }


        [Route("/Admin/C/Delete/{id}")]
        public async Task Delete(int id)
        {
            Category category = _context.CategoryRepository.GetAllAsync(p => p.CategoryId == id,
                p => p.SubCategories).Result.FirstOrDefault();

            if (category != null)
            {
                var subitems = category.SubCategories;

                if (subitems.Count > 0)
                {
                    foreach (var item in subitems)
                    {
                        var threeitems = _context.CategoryRepository
                            .GetAllAsync(c => c.ParentId == item.CategoryId)
                            .Result
                            .ToList();

                        if (threeitems.Count > 0)
                        {
                            _context.CategoryRepository
                                .GetAllAsync(c => c.ParentId == item.CategoryId)
                                .Result
                                .ToList()
                                .ForEach(c => _context.CategoryRepository.RemoveAsync(c));
                        }

                        await _context.CategoryRepository.RemoveAsync(item);
                    }
                }

                await _context.CategoryRepository.RemoveAsync(category);
            }
        }

        public async Task<IActionResult> GetProducts(int CategoryId = 0, int Page = 1)
        {
            int countInPage = 10;

            if (CategoryId == 0) //دسته بندی نشده ها
            {
                var products = _context.ProductRepository.GetAllAsync(null,
                                                                  includ => includ.Categories).Result;

                var productsWithOutCategory = products.Where(p => p.Categories.Any()==false)
                                            .Skip((Page - 1) * countInPage).Take(countInPage)
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Displayed = p.Displayed,
                    Id = p.Id,
                    Image = p.Image,
                    Price = p.Price,
                    HasWarranty = p.Warranty != null ? true : false
                }).ToList();

                var model = new ProductsCateogryDto
                {
                    Page = Page,
                    CountAllItems = productsWithOutCategory.Count(),
                    CounInPage = countInPage,
                    Cateogry = new CateogryDto
                    {
                        Id = 0,
                        Name = "محصولات دسته بندی نشده"
                    },
                    Products = productsWithOutCategory
                };


                return View(model);
            }
            else//دسته بندی شده ها
            {
                Category category = _context.CategoryRepository.GetAllAsync(p => p.CategoryId == CategoryId,
                                                                  includ => includ.Products).Result.FirstOrDefault();

                if (category == null)
                {
                    return NotFound();
                }

                var model = new ProductsCateogryDto
                {
                    Page = Page,
                    CountAllItems = category.Products.Count(),
                    CounInPage = countInPage,
                    Cateogry = new CateogryDto
                    {
                        Id = category.CategoryId,
                        Name = category.Name
                    }
                };

                model.Products = category.Products.Skip((Page - 1) * model.CounInPage).Take(model.CounInPage)
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
        }

    }
}
