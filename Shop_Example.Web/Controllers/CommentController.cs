using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Comment;
using Shop_Example.Entities.Models;
using Shop_Example.Entities.Products.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Controllers
{
    [Route("/Product/{ProductId}/")]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        public CommentController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [Route("AddComment")]
        public async Task<IActionResult> Create(int ProductId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);

            if (product == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            //اگه کاربر قبلا برای این محصول کامنت نگذاشته بود
            //if (!(_unitOfWork.CommentRepository.GetAllAsync(p => p.ProductId == product.Id && p.UserId == userId).Result.Any()))
            //{

            ViewData["ProductInfo"] = await GetModelInfoProduct(product);

            return View();
            //}
            //else
            //{
            //    return BadRequest();
            //}
        }

        [Route("AddComment")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCommentDto modelComment, IFormCollection keyValues)
        {

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(modelComment.ProductId);

            if (product == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid == false)
            {
                ViewData["ProductInfo"] = await GetModelInfoProduct(product);

                return View(modelComment);
            }


            var goodPoints = keyValues["comment[advantages][]"];
            var badPoints = keyValues["comment[disadvantages][]"];

            List<Point> allPoints = goodPoints.Select(p => new Point
            {
                Text = p,
                TypePoint = TypePoint.Strength
            }).ToList();

            allPoints.AddRange(badPoints.Select(p => new Point
            {
                Text = p,
                TypePoint = TypePoint.Weak
            }));

            bool result = await _unitOfWork.CommentRepository.AddAsync(new Comment()
            {
                Title = modelComment.Title,
                Text = modelComment.Text,
                Confirmation = true,//موقت
                UserId = _userManager.GetUserId(User),
                ProductId = modelComment.ProductId,
                Suggestion = modelComment.Suggestion,
                Stars = new Stars
                {
                    Ability = modelComment.Ability,
                    Affordable = modelComment.Affordable,
                    Beauty = modelComment.Beauty,
                    EasyUse = modelComment.EasyUse,
                    Innovation = modelComment.Innovation,
                    QualityBuild = modelComment.QualityBuild
                },
                Points=allPoints
            });

            return RedirectToAction("Detail", "Product", new { ProductId = product.Id });
        }



        [NonAction]
        public async Task<InfoProductDto> GetModelInfoProduct(Product product)
        {

            if (product == null)
            {
                return null;
            }

            var productInfo = new InfoProductDto
            {
                ProductModel = product.Model,
                ProductName = product.Name,
                ProductImage = product.Image
            };

            return productInfo;
        }
    }
}
