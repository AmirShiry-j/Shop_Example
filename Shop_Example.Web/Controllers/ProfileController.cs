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

namespace Shop_Example.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public ProfileController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
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
        //بعد از اضافه کردن محصولات
        public IActionResult Order()
        {

            return View();
        }

        public IActionResult Comment()
        {
            return View();
        }

        public IActionResult Favorites()
        {
            return View();
        }


    }
}
