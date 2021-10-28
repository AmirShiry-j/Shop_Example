using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Dtoes.Product;
using Shop_Example.Entities.Models;
using Shop_Example.Entities.Products.Comments;
using Shop_Example.Tools.TimeAndDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Component.Comment
{
    public class ShowCommentsViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        private readonly Time _time;

        public ShowCommentsViewComponent(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;

            _time = new Time();
        }

        public async Task<IViewComponentResult> InvokeAsync(int ProductId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);

            var comments = await _unitOfWork.CommentRepository.GetAllAsync(p => p.ProductId == ProductId,
                                                                    include => include.Points,
                                                                    include => include.Stars,
                                                                    include => include.User);

            var model = new CommentViewModel()
            {
                ProductId = product.Id,
                ModelNameProduct = product.Model
            };

            if (comments != null)
            {
                model.Comments = comments.Select(p => new CommentDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    Suggestion = p.Suggestion,
                    UserFullName = _userManager.GetUserAsync(UserClaimsPrincipal).Result.FullName,
                    GoodPoints = _unitOfWork.PointRepository.GetAllAsync(t => t.TypePoint == TypePoint.Strength && t.CommentId == p.Id).Result.Select(p => p.Text).ToList(),
                    BadsPoints = _unitOfWork.PointRepository.GetAllAsync(t => t.TypePoint == TypePoint.Weak && t.CommentId == p.Id).Result.Select(p => p.Text).ToList(),
                    DateCreateShamsi = _time.ToShamsi(p.DateCreate),
                    CountStars = Convert.ToByte(p.Stars.AverageStars),

                }).ToList();

                var stars = comments.Select(p => p.Stars).ToList();

                model.QualityAverages = new QualityAveragesDto
                {
                    Ability = GetPercentage(stars.Average(p => p.Ability)),
                    Affordable = GetPercentage(stars.Average(p => p.Affordable)),
                    Innovation = GetPercentage(stars.Average(p => p.Innovation)),
                    EasyUse = GetPercentage(stars.Average(p => p.EasyUse)),
                    Beauty = GetPercentage(stars.Average(p => p.Beauty)),
                    QualityBuild = GetPercentage(stars.Average(p => p.QualityBuild))
                };

            }

            return View("Component/ShowComments.cshtml", model);
        }

        public byte GetPercentage(double qulity)
        {
            //5 is Maximum
            return Convert.ToByte((qulity * 100) / 5);
        }
    }
}
