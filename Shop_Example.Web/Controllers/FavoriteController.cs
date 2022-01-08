using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Entities.Models;
using Shop_Example.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Controllers
{
    [Authorize]
    [Route("/{Controller}/{Action}/{ProductId}")]
    public class FavoriteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        public FavoriteController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<bool> Add(int ProductId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);

            if (product == null)
                return false;

            var userId = _userManager.GetUserId(User);

            var HasRecordForUser =
                _unitOfWork.FavoriteRepository.GetAllAsync(p => p.UserId == userId &&
                                                           p.ProductId == product.Id)
                                                           .Result.FirstOrDefault();
            if (HasRecordForUser == null)
            {
                var newFavorite = new Favorite
                {
                    ProductId = product.Id,
                    UserId = userId
                };

                return await _unitOfWork.FavoriteRepository.AddAsync(newFavorite);
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        public async Task<bool> Remove(int ProductId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(ProductId);

            if (product == null)
                return false;

            var userId = _userManager.GetUserId(User);

            var favorite =
                _unitOfWork.FavoriteRepository.GetAllAsync(p => p.UserId == userId &&
                                                           p.ProductId == product.Id)
                                                           .Result.FirstOrDefault();

            if (favorite != null)
            {
                return await _unitOfWork.FavoriteRepository.RemoveAsync(favorite);
            }
            else
            {
                return false;
            }
        }
    }
}
