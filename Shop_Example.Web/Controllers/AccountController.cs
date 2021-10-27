using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.Dtoes.Account;
using Shop_Example.Tools.EmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Example.Entities.Models;

namespace Shop_Example.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        private readonly IEmailService _emailService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _emailService = emailService;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterDto register)
        {
            if (ModelState.IsValid == false)
            {
                return View(register);
            }

            _signInManager.SignOutAsync();

            User newUser = new User
            {
                UserName = register.Email,
                Email = register.Email,
                FullName = register.FullName,

                EmailConfirmed = true//موقت ایمیل ها تایید بشن(برای ثبت کارابران فیک توسط خودمون)
            };

            var resultRegister = _userManager.CreateAsync(newUser, register.Password).Result;

            if (resultRegister.Succeeded)
            {

                //برای تایید حساب در هنگام ثبت نام واقعی
                //2 خط پایین

                //TempData["Email"] = register.Email;
                //return RedirectToAction("ConfirmEmail");

                _signInManager.SignInAsync(newUser, false).Wait();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                string Message = "";
                foreach (var Error in resultRegister.Errors)
                {
                    Message += Error.Description;
                }

                ModelState.AddModelError("", Message);

                return View(register);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDto login)
        {
            if (ModelState.IsValid == false)
            {
                return View(login);
            }

            _signInManager.SignOutAsync();

            User user = _userManager.FindByEmailAsync(login.Email).Result;

            if (user == null)
            {
                ModelState.AddModelError("", "کاربری با این ایمیل یافت نشد");

                return View(login);
            }

            var resultLogin = _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistens, true).Result;


            if (resultLogin.Succeeded)
            {

                return RedirectToAction("Index", "Home");

            }
            else if (resultLogin.IsLockedOut)
            {

                ModelState.AddModelError("", "حساب کاربری شما قفل شد");

                return View(login);

            }
            else if (resultLogin.RequiresTwoFactor)//برای ورود دو مرحله ای
            {

                TempData["UserId"] = user.Id;
                TempData["IsPersistans"] = login.IsPersistens;

                return RedirectToAction("TwoFactorLogin");
            }
            else if (resultLogin.IsNotAllowed)//حساب کاربری تایید نشده
            {
                //برای تایید حساب

                TempData["Email"] = user.Email;

                return RedirectToAction("ConfirmEmail");

            }
            else
            {

                ModelState.AddModelError("", "پسورد وارد شده صحیح نیست");

                return View(login);

            }
        }

        [Authorize]
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ConfirmEmail()
        {
            try
            {
                string emailUser = TempData["Email"].ToString();

                if (emailUser == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                var user = _userManager.FindByEmailAsync(emailUser).Result;

                if (user == null)
                {
                    return RedirectToAction("Error", "Home");
                }



                var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                string redirectUrl = Url.Action("VerifyEmail", "Account", new { UserId = user.Id, token = token }, Request.Scheme);

                string bodyEmail = $"لطفا برای فعالسازی حساب خود در سایت کالا مارکت بر روی لینک زیر کلیک کنید. <br/> <a href='{redirectUrl}'><h3> تایید حساب کاربری </h3></a>";

                _emailService.SendEmail(user.Email, bodyEmail, "تایید حساب");

                return View("ConfirmEmail", user.Email);
            }
            catch
            {

                return RedirectToAction("Error", "Home");
            }
        }
        public IActionResult VerifyEmail(string UserId, string token)
        {
            if (UserId == null || token == null)
            {
                return View("FailedConfirmEmail");
            }

            var user = _userManager.FindByIdAsync(UserId).Result;

            if (user == null)
            {
                return View("FailedConfirmEmail");

            }

            var resultConfirmEmail = _userManager.ConfirmEmailAsync(user, token).Result;

            if (resultConfirmEmail.Succeeded)
            {
                _signInManager.SignInAsync(user, false).Wait();

                return View("SuccessConfirmEmail");
            }
            else
            {
                return View("FailedConfirmEmail");
            }
        }



        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordDto forgetPassword)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(forgetPassword);
                }

                var user = _userManager.FindByEmailAsync(forgetPassword.Email).Result;

                if (user == null)
                {
                    ModelState.AddModelError("", "کاربری با این ایمیل یافت نشد");

                    return View(forgetPassword);
                }

                var resultEmailConfirm = _userManager.IsEmailConfirmedAsync(user).Result;

                if (resultEmailConfirm == false)//ایمیل تایید نشده
                {
                    //بره ایمیلو تایید کنه

                    TempData["Email"] = user.Email;

                    return RedirectToAction("ConfirmEmail");
                }


                var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                var redirectUrl = Url.Action("ResetPassword", "Account", new { UserId = user.Id, token = token }, Request.Scheme);

                string bodyEmail = $"برای بازیابی رمز عبور خود در سایت کالا مارکت بر روی لینک زیر کلیک کنید <br/> <a href={redirectUrl}> <h3> بازیابی رمز عبور </h3> </a>";

                _emailService.SendEmail(user.Email, bodyEmail, "بازیابی رمز عبور");

                return View("SendEmailResetPassword", user.Email);
            }
            catch
            {

                return RedirectToAction("Error", "Home");
            }
        }


        public IActionResult ResetPassword(string UserId, string token)
        {

            ResetPasswordDto resetPassword = new ResetPasswordDto
            {
                UserId = UserId,
                Token = token
            };
            return View(resetPassword);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDto resetPassword)
        {
            if (ModelState.IsValid == false)
            {
                return View(resetPassword);
            }

            var user = _userManager.FindByIdAsync(resetPassword.UserId).Result;

            if (user == null)
            {
                return View("FailedResetPassword");
            }

            var result = _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password).Result;

            if (result.Succeeded)
            {
                return View("SuccessResetPassword");
            }
            else
            {
                string Message = "";

                foreach (var error in result.Errors)
                    Message += error.Description;

                ModelState.AddModelError("", Message);

                return View(resetPassword);
            }
        }

        public IActionResult TwoFactorLogin()
        {

            try
            {
                string userId = TempData["UserId"].ToString();

                bool? IsPersistans = TempData["IsPersistans"] as bool?;

                if (userId == null || IsPersistans == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                var user = _userManager.FindByIdAsync(userId).Result;

                if (user == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                TwoFactorDto twoFactorLogin = new TwoFactorDto
                {
                    IsPersistans = (bool)IsPersistans,
                    Email = user.Email
                };

                //پرووایدر ینی سرویسایی که میتونیم باهاش یه پیام ارسال کنیم
                //ینی هر چیزایی که از کاربر تایید شده : ایمیل کاربر یا شماره موبایل اون
                var providers = _userManager.GetValidTwoFactorProvidersAsync(user).Result;
                if (providers.Contains("Email"))
                {
                    string codeEmail = _userManager.GenerateTwoFactorTokenAsync(user, "Email").Result;

                    string bodyEmail = $"لطفا برای تکمیل ورود دو مرحله به حساب خود کد زیر را در فرم مربوطه وارد کنید <br/> <h2>{codeEmail}</h2>";
                    _emailService.SendEmail(user.Email, bodyEmail, "ورود دو مرحله ای");

                    twoFactorLogin.Provider = "Email";

                    return View(twoFactorLogin);
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            catch
            {


                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult TwoFactorLogin(TwoFactorDto twoFactor)
        {
            if (ModelState.IsValid == false)
            {
                return View(twoFactor);
            }

            var user = _signInManager.GetTwoFactorAuthenticationUserAsync().Result;

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var resultTwoFactor = _signInManager.TwoFactorSignInAsync(twoFactor.Provider, twoFactor.Code, twoFactor.IsPersistans, false).Result;

            if (resultTwoFactor.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (resultTwoFactor.IsLockedOut)
            {
                ModelState.AddModelError("", "حساب کاربری شما قفل است");

                return View(twoFactor);
            }
            else
            {
                ModelState.AddModelError("", "کد وارد شده صحیح نیست");

                return View(twoFactor);
            }
        }


        [Authorize]//این اتریبیوت : ینی فقط در صورتی به این اکشن دسترسی داره که لاگین کرده باشه
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordDto changePassword)
        {
            if (ModelState.IsValid == false)
            {
                return View(changePassword);
            }

            var user = _userManager.GetUserAsync(User).Result;

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var result = _userManager.ChangePasswordAsync(user, changePassword.NowPassword, changePassword.NewPassword).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                string Message = "";

                foreach (var error in result.Errors)
                    Message += error.Description;

                ModelState.AddModelError("", Message);

                return View(changePassword);
            }
        }
        
    }
}
