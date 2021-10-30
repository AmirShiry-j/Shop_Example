using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Entities.Models;
using Shop_Example.Entities.Products.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Controllers
{

    [Authorize]
    [Route("/{Controller}/{Action}/{CommentId}")]
    public class HelpfulController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        public HelpfulController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<bool> AddIsHelpful(long CommentId)
        {
            var userId = _userManager.GetUserId(User);
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(CommentId);

            if (comment == null)
            {
                return false;
            }

            if (RemovePastRecords(userId, CommentId))
            {
                var newIsHelpful = new Helpful
                {
                    UserId = userId,
                    CommentId = CommentId,
                    WasHelpful = true
                };

                return await _unitOfWork.HelpfulRepository.AddAsync(newIsHelpful);
            }
            else
            {
                return false;
            }
        }

        [HttpDelete]
        public async Task<bool> RemoveIsHelpful(long CommentId)
        {
            var userId = _userManager.GetUserId(User);
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(CommentId);

            if (comment == null)
            {
                return false;
            }

            if (RemovePastRecords(userId, CommentId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<bool> AddNotHelpful(long CommentId)
        {

            var userId = _userManager.GetUserId(User);
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(CommentId);

            if (comment == null)
            {
                return false;
            }

            if (RemovePastRecords(userId, CommentId))
            {
                var newNotHelpful = new Helpful
                {
                    UserId = userId,
                    CommentId = CommentId,
                    WasHelpful = false
                };

                return await _unitOfWork.HelpfulRepository.AddAsync(newNotHelpful);
            }
            else
            {

                return false;
            }
        }
        [HttpDelete]
        public async Task<bool> RemoveNotHelpful(long CommentId)
        {
            var userId = _userManager.GetUserId(User);
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(CommentId);

            if (comment == null)
            {
                return false;
            }

            if(RemovePastRecords(userId, CommentId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        [NonAction]
        public bool RemovePastRecords(string UserId, long CommentId)
        {
            var helpfulRecords = _unitOfWork.HelpfulRepository.GetAllAsync(p => p.UserId == UserId &&
                                                                           p.CommentId == CommentId).Result;

            if (helpfulRecords != null)
            {
                return _unitOfWork.HelpfulRepository.RemoveReangeAsync(helpfulRecords.ToList()).Result;
            }

            return true;
        }
    }
}
