using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Product;
using Shop_Example.Entities.Models;
using Shop_Example.Entities.Products.Comments;
using Shop_Example.Web.Tools.DiscountHelper;
using Shop_Example.Web.Tools.GetAvgStarsProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Controllers
{

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Discount _discount;
        private readonly AvgStarsProduct _avgStarsProduct;

        private readonly UserManager<User> _userManager;
        public ProductController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _discount = new Discount();

            _userManager = userManager;
            _avgStarsProduct = new AvgStarsProduct(unitOfWork);
        }

        public async Task<IActionResult> Index(int CategoryId = 0, string Search = "",
                                                 int Page = 1)
        {
            //Get Product With Where Category And SearchKey ...
            //Start...
            IEnumerable<Product> products;

            if (CategoryId != 0)//Take Products With This Category
            {
                products = await _unitOfWork.ProductRepository.GetAllAsync(p => p.Categories.Any(p => p.CategoryId == CategoryId) && p.Displayed,
                                                                           include => include.Favorites);

            }
            else
            {
                products = await _unitOfWork.ProductRepository.GetAllAsync(p => p.Displayed,
                                                                           include => include.Favorites);
            }

            if (!string.IsNullOrEmpty(Search))
                if (products != null)//Search With Word
                    products = products.Where(p => p.Name.ToLower().Contains(Search) ||
                                                        p.Model.ToLower().Contains(Search) ||
                                                        p.Brand.ToLower().Contains(Search));
            //...End

            int totalRecords = 0;

            //Ordering And Mapping And Buil Model Page ....
            //Start...
            var model = new ListProductsViewModel
            {
                Page = Page
            };

            //Init SearchKeyName and CategoryName To Model
            if (!(string.IsNullOrEmpty(Search)))
            {
                model.SearchKeyName = Search;
            }
            if (CategoryId == 0)
            {
                model.CategoryName = "همه دسته بندی ها";
            }
            else
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(CategoryId);

                if (category != null)
                {
                    model.CategoryName = category.Name;
                }
            }

            if (products != null)//Take Products for This Page
            {
                totalRecords = products.Count();

                model.TotalRecords = totalRecords;


                //Prop Order By Views 

                var productsOrderByViews = products.OrderByDescending(p => p.Views).Skip((Page - 1) * 20).Take((Page * 20));

                model.ProductsOrderByViews = MapProductsToDto(productsOrderByViews);


                //Prop Order By Cheaper 

                var productsOrderByCheaper = products.OrderBy(p => p.Price).Skip((Page - 1) * 20).Take((Page * 20));

                model.ProductsOrderByCheaper = MapProductsToDto(productsOrderByCheaper);


                //Prop Order By Expensive 

                var productsOrderByExpensive = products.OrderByDescending(p => p.Price).Skip((Page - 1) * 20).Take((Page * 20));

                model.ProductsOrderByExpensive = MapProductsToDto(productsOrderByExpensive);


                //Prop Order By Date 

                var productsOrderByDate = products.OrderByDescending(p => p.TimeCreate).Skip((Page - 1) * 20).Take((Page * 20));

                model.ProductsOrderByDate = MapProductsToDto(productsOrderByDate);


                //Prop Order By Favorite 

                var productsOrderByFavorite = products.OrderByDescending(p => p.Favorites.Count).Skip((Page - 1) * 20).Take((Page * 20));

                model.ProductsOrderByFavorite = MapProductsToDto(productsOrderByFavorite);



                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        [Route("/Product/Detail")]
        public async Task<IActionResult> Detail(int ProductId, int Page = 1)
        {

            var product = _unitOfWork.ProductRepository.GetAllAsync(p => p.Id == ProductId && p.Displayed == true,
                                                        p => p.ProductImages,
                                                        p => p.ProductFeatures,
                                                        p => p.ProductTages,
                                                        p => p.Categories,
                                                        p => p.Warranty).Result.FirstOrDefault();
         

            if (product == null)
            {
                return RedirectToRoute("Error", "Home");
            }

            //Plus Product Views
            product.Views++;
            await _unitOfWork.ProductRepository.UpdateAsync(product);


            decimal avgStars = 1;//پیش فرض 

            product.Comments = _unitOfWork.CommentRepository.GetAllAsync(p => p.ProductId == product.Id, p => p.Stars).Result.ToList();

            if (product.Comments != null && product.Comments.Any())
            {
                //میانگین تعداد رای ها به محصول از نظرات کاربران
                avgStars = product.Comments.Select(p => p.Stars).ToList().Average(p => p.AverageStars);
            }

            bool hasFavorite = false;
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var favorite = _unitOfWork.FavoriteRepository.GetAllAsync(p => p.ProductId == product.Id &&
                                                                             p.UserId == userId)
                                                                             .Result.FirstOrDefault();
                if (favorite != null)
                {
                    hasFavorite = true;
                }
            }

            var model = new DatailsProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Image = product.Image,
                Model = product.Model,
                Description=product.Description,
                Page = Page,
                Favorite = hasFavorite,
                ShowedPrice = _discount.GetShowedPrice(product.Price, product.Discount),
                LinedPrice = _discount.GetLinedPrice(product.Price, product.Discount),
                Warranty = product.Warranty != null ? new WarrantyDto { Id = product.Warranty.Id, Name = product.Warranty.Name } : null,
                Tages = product.ProductTages.Select(t => t.Value).ToList(),
                ProductFeatures = product.ProductFeatures.Select(f => new FeatureDto { DisplayName = f.DisplayName, Value = f.Value }).ToList(),
                SrcImages = product.ProductImages.Select(m => m.Image).ToList(),
                Categories = product.Categories.Select(c => new CategoryDto { Id = c.CategoryId, Name = c.Name }).ToList(),

                ProductStarts = Convert.ToByte(avgStars),
            };

            return View(model);
        }

        [NonAction]
        public List<ProductDto> MapProductsToDto(IEnumerable<Product> Products)
        {
            return Products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image,
                Price = _discount.GetShowedPrice(p.Price, p.Discount),
                HasDiscount = (p.Discount == null || p.Discount == 0) ? false : true,
                Discount = (p.Discount == null || p.Discount == 0) ? (byte)0 : (byte)p.Discount,
                Stars = _avgStarsProduct.GetAvgStars(p.Id)
            }).ToList();
        }
    }
}
