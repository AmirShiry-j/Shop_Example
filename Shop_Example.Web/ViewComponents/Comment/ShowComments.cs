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
    [ViewComponent]
    public class ShowComments : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        private readonly Time _time;

        public ShowComments(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;

            _time = new Time();
        }

        public async Task<IViewComponentResult> InvokeAsync(int ProductId, int Page = 1)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);

            var comments = await _unitOfWork.CommentRepository.GetAllAsync(p => p.ProductId == ProductId,
                                                                    include => include.Points,
                                                                    include => include.Stars,
                                                                    include => include.User);

            int countInPage = 5;

            var model = new CommentViewModel()
            {
                ProductId = product.Id,
                ModelNameProduct = product.Model,
                CountInPage=countInPage
            };

            if (comments != null)
            {
                List<Helpful> helpfulCommentsOfUser = new List<Helpful>();

                bool IsAuthenticated = User.Identity.IsAuthenticated;
                if (IsAuthenticated)
                {
                    var userId = _userManager.GetUserId(UserClaimsPrincipal);

                    helpfulCommentsOfUser = _unitOfWork.HelpfulRepository.GetAllAsync(p => p.UserId == userId).Result.ToList();

                }

                model.Comments = comments.OrderByDescending(p => p.DateCreate)
                    .Skip((Page - 1) * countInPage).Take(countInPage).Select(p => new CommentDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Text = p.Text,
                        Suggestion = p.Suggestion,
                        UserFullName = _userManager.FindByIdAsync(p.UserId).Result.FullName,
                        GoodPoints = _unitOfWork.PointRepository.GetAllAsync(t => t.TypePoint == TypePoint.Strength && t.CommentId == p.Id).Result.Select(p => p.Text).ToList(),
                        BadsPoints = _unitOfWork.PointRepository.GetAllAsync(t => t.TypePoint == TypePoint.Weak && t.CommentId == p.Id).Result.Select(p => p.Text).ToList(),
                        DateCreateShamsi = _time.ToShamsi(p.DateCreate),
                        CountStars = Convert.ToByte(p.Stars.AverageStars),

                        CountIsHelpful = _unitOfWork.HelpfulRepository.GetAllAsync(t => t.WasHelpful == true && t.CommentId == p.Id).Result.Count(),
                        IsHelpfulByUser = IsAuthenticated == false ? false : helpfulCommentsOfUser.Any(t => t.CommentId == p.Id && t.WasHelpful),

                        CountNoHelpful = _unitOfWork.HelpfulRepository.GetAllAsync(t => t.WasHelpful == false && t.CommentId == p.Id).Result.Count(),
                        NotHelpfulByUser = IsAuthenticated == false ? false : helpfulCommentsOfUser.Any(t => t.CommentId == p.Id && t.WasHelpful == false),

                    }).ToList();

                model.CountAllComments = comments.Count();
                model.Page = Page;

                var stars = comments.Select(p => p.Stars).ToList();
                if (stars != null && stars.Any())
                {
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
                else
                {
                    //Default Values
                    model.QualityAverages = new QualityAveragesDto
                    {
                        Ability = 21,
                        Affordable = 21,
                        Innovation = 21,
                        EasyUse = 21,
                        Beauty = 21,
                        QualityBuild = 21
                    };
                }
            }

            return View("/Views/Product/ViewComponents/ShowComments.cshtml", model);
        }

        public byte GetPercentage(double qulity)
        {
            //5 is Maximum
            return Convert.ToByte((qulity * 100) / 5);
        }
    }
}
