using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.Dtoes.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Example.Entities.Models;
using Shop_Example.DataLayer.Repositorys.UnitOfWorkRepository.Interface;
using Shop_Example.Web.Tools.GetAvgStarsProduct;
using Microsoft.AspNetCore.Http;
using System.IO;
using Shop_Example.Web.Tools.CheckImageValidation;

namespace Shop_Example.Web.Controllers
{
    [Authorize]
    [Route("/{Controller}/{Action}/")]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AvgStarsProduct _avgStarsProduct;
        private readonly IUnitOfWork _unitOfWork;

        public ProfileController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;

            _avgStarsProduct = new AvgStarsProduct(_unitOfWork);
        }


        public async Task<JsonResult> UploadImage(IFormFile fileToUpload)
        {
            var model = new UpdateImageProfileDto();

            if (fileToUpload == null && !ValidationImage.Validate(fileToUpload))
            {
                return Json(new UpdateImageProfileDto()
                {
                    IsSuccess = false
                });
            }

            var user = _userManager.GetUserAsync(User).Result;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Images/ProfileImages/");

            if (user.ImageProfileName != null)
            {
                if (System.IO.File.Exists(path + user.ImageProfileName))
                {
                    System.IO.File.Delete(path + user.ImageProfileName);
                }

                user.ImageProfileName = null;
            }

            string imageName = Guid.NewGuid() + Path.GetExtension(fileToUpload.FileName);

            using (var fileStream = new FileStream(path + imageName, FileMode.Create))
            {
                fileToUpload.CopyTo(fileStream);
            }

            user.ImageProfileName = imageName;

            var resultUpdateUser = await _userManager.UpdateAsync(user);

            if (resultUpdateUser.Succeeded)
            {
                return Json(new UpdateImageProfileDto()
                {
                    IsSuccess = true,
                    ImageSrc = imageName
                });
            }
            else
            {
                return Json(new UpdateImageProfileDto()
                {
                    IsSuccess = false
                });
            }
        }

        public async Task<bool> DeleteImage()
        {
            var user = _userManager.GetUserAsync(User).Result;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Images/ProfileImages/");

            if (user.ImageProfileName != null)
            {
                if (System.IO.File.Exists(path + user.ImageProfileName))
                {
                    System.IO.File.Delete(path + user.ImageProfileName);
                }

                user.ImageProfileName = null;

                var resultUpdateUser = await _userManager.UpdateAsync(user);

                return resultUpdateUser.Succeeded;
            }


            return false;
        }

        public async Task<IActionResult> Index()
        {
            var user = _userManager.GetUserAsync(User).Result;

            var fullInfoUser = new FullInfoUserDto
            {
                FullName = user.FullName,
                Email = user.Email,
                IsConfirmedPhoneNumber = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber,
                TwoFactorLogin = user.TwoFactorEnabled
            };

            return View(fullInfoUser);
        }
        public async Task<IActionResult> InfoAccount()
        {
            var user = _userManager.GetUserAsync(User).Result;

            var editInfoUser = new EditUserInfoDto
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                TwoFactorLogin = user.TwoFactorEnabled
            };

            return View(editInfoUser);
        }

        [HttpPost]
        public async Task<IActionResult> InfoAccount(EditUserInfoDto editUserInfo)
        {
            if (ModelState.IsValid == false)
            {
                return View(editUserInfo);
            }

            var user = _userManager.GetUserAsync(User).Result;
            user.FullName = editUserInfo.FullName;
            if (user.Email != editUserInfo.Email)
            {
                user.EmailConfirmed = false;
            }
            user.Email = editUserInfo.Email;
            user.PhoneNumber = editUserInfo.PhoneNumber;
            user.TwoFactorEnabled = editUserInfo.TwoFactorLogin;

            var resultUpdateUser = _userManager.UpdateAsync(user).Result;

            if (resultUpdateUser.Succeeded)
            {
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                string message = "";

                foreach (var error in resultUpdateUser.Errors)
                    message += error.Description;

                ModelState.AddModelError("", message);

                return View(editUserInfo);
            }

        }

        public async Task<IActionResult> Address()
        {
            var userActive = await _userManager.GetUserAsync(User);

            if (userActive != null)
            {
                var address =
                    _unitOfWork.AddressRepository
                    .GetAllAsync(p => p.UserId == userActive.Id, p => p.City, p => p.City.United).Result.SingleOrDefault();

                var model = new AddressInfoDto();

                if (address != null)
                {
                    var city = address.City;
                    var united = city.United;

                    model.FullAddress = address.FullAddress;
                    model.RecipientName = address.RecipientName;
                    model.PostalCode = address.PostalCode;
                    model.UnitedAndCityName = united?.Name + " ، " + city?.Name;
                    model.AddressId = address.AddressId;
                }
                else
                    model = null;

                return View(model);
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Comments()
        {
            var userId = _userManager.GetUserId(User);
            var comments = await _unitOfWork.CommentRepository.GetAllAsync(p => p.UserId == userId,
                                                                           include => include.Stars,
                                                                           include => include.Product);
            if (comments != null)
            {
                comments = comments.OrderByDescending(p => p.DateCreate);

                var model = new CommentsViewModel
                {
                    Comments = comments.Select(p => new CommentDto
                    {
                        CommentId = p.Id,
                        CommentText = p.Text,
                        IsConfirmed = p.Confirmation,
                        ProductId = p.ProductId,
                        ProductImage = p.Product.Image,
                        ProductName = p.Product.Name,
                        CommentStars = (byte)p.Stars.AverageStars
                    }).ToList()
                };

                return View(model);
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Favorites()
        {
            var userId = _userManager.GetUserId(User);
            var favorites = await _unitOfWork.FavoriteRepository.GetAllAsync(p => p.UserId == userId,
                                                                             include => include.Product);

            if (favorites != null)
            {
                favorites = favorites.Reverse();

                var model = new FavoritesViewModel
                {
                    Favorites = favorites.Select(p => new FavoriteDto
                    {
                        FavoriteId = p.Id,
                        ProductName = p.Product.Name,
                        ProductImage = p.Product.Image,
                        ProductId = p.ProductId,
                        ProductPrice = p.Product.Price,
                        ProductAvgStars = _avgStarsProduct.GetAvgStars(p.ProductId)
                    }).ToList()
                };

                return View(model);
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Order()
        {

            return View();
        }
    }
}
