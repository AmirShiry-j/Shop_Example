using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Tools.TimeAndDate;
using Shop_Example.Dtoes.Admin.Comment;
using System;
using Shop_Example.Entities.Products.Comments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shop_Example.Web.Tools.Agragate;
using Microsoft.AspNetCore.Identity;
using Shop_Example.Entities.Models;

namespace Shop_Example.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    [Route("{Area}/{Controller}/{Action}/")]
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly Time _time;
        private readonly Aggregate _aggregate;
        public CommentController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;

            _time = new Time();
            _aggregate = new Aggregate();
        }

        public async Task<IActionResult> Index()
        {
            var allComments = await _unitOfWork.CommentRepository.GetAllAsync(p => p.Confirmation == false
                                                                                , includ => includ.Stars
                                                                                , includ => includ.User
                                                                                , includ => includ.Points
                                                                                , includ => includ.Product);


            var comments = allComments.OrderByDescending(p => p.DateCreate).Select(p => new CommentDto
            {
                Id = p.Id,
                Title = p.Title,
                Body = p.Text,
                AvgStars = (byte)p.Stars.AverageStars,
                StreaghtPoint = _aggregate.GetAggregatePoint(p.Points?.Where(p => p.TypePoint == TypePoint.Strength)?.Select(p => p.Text)?.ToList()),
                WeakPoint = _aggregate.GetAggregatePoint(p.Points?.Where(p => p.TypePoint == TypePoint.Weak)?.Select(p => p.Text)?.ToList()),
                FullName = p.User.FullName,
                UserId = p.UserId,
                ProductId = p.ProductId,
                ProductName = p.Product.Name,
                TimeCrate = _time.GetCurrentTime(p.DateCreate),
                IsBlocked = p.User.IsBlocked


            }).ToList();


            return View(comments);
        }
        [Route("~")]
        public async Task<IActionResult> AllComments(int Page = 1)
        {
            var allComments = await _unitOfWork.CommentRepository.GetAllAsync(null
                                                                                , includ => includ.Stars
                                                                                , includ => includ.User
                                                                                , includ => includ.Points
                                                                                , includ => includ.Product);
            int countInPage = 15;

            var model = new AllCommentsDto
            {
                Page = Page,
                CountComments = allComments.Count(),
                CountInPage = countInPage
            };

            var commentsThisPage = allComments.OrderByDescending(p => p.DateCreate)
                .Skip((Page - 1) * countInPage)
                .Take(countInPage)
                .ToList();

            if (commentsThisPage != null && commentsThisPage.Any())
            {
                model.Comments = commentsThisPage.Select(p => new CommentDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Body = p.Text,
                    AvgStars = (byte)p.Stars.AverageStars,
                    FullName = p.User.FullName,
                    UserId = p.UserId,
                    ProductId = p.ProductId,
                    ProductName = p.Product.Name,
                    TimeCrate = _time.GetCurrentTime(p.DateCreate),
                    IsConfirm = p.Confirmation,
                    IsBlocked = p.User.IsBlocked,
                    StreaghtPoint = _aggregate.GetAggregatePoint(p.Points?.Where(p => p.TypePoint == TypePoint.Strength)?.Select(p => p.Text)?.ToList()),
                    WeakPoint = _aggregate.GetAggregatePoint(p.Points?.Where(p => p.TypePoint == TypePoint.Weak)?.Select(p => p.Text)?.ToList()),
                }).ToList();

            }


            return View(model);
        }


        [Route("{CommentId}")]
        public async Task<bool> Confirm(long CommentId)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(CommentId);

            if (comment == null)
            {
                return false;
            }

            comment.Confirmation = true;

            var resultEdit = await _unitOfWork.CommentRepository.UpdateAsync(comment);

            return resultEdit;
        }

        [Route("{CommentId}")]
        public async Task<bool> Delete(long CommentId)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(CommentId);

            if (comment == null)
            {
                return false;
            }

            var resultDelete = await _unitOfWork.CommentRepository.RemoveAsync(comment);

            return resultDelete;
        }

        [Route("{UserId}")]
        public async Task<bool> BlockUser(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            user.IsBlocked = true;

            var resultUpdate = await _userManager.UpdateAsync(user);

            return resultUpdate.Succeeded;
        }

        [Route("{UserId}")]
        public async Task<bool> UnBlockUser(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            user.IsBlocked = false;

            var resultUpdate = await _userManager.UpdateAsync(user);

            return resultUpdate.Succeeded;
        }
    }
}
