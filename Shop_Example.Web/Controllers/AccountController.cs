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
using Microsoft.Extensions.Logging;

namespace Shop_Example.Web.Controllers
{
    [Route("/{Controller}/{Action}/")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _logger;

        private readonly IEmailService _emailService;
        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailService,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if (ModelState.IsValid == false)
            {
                return View(register);
            }

            await _signInManager.SignOutAsync();

            User newUser = new User
            {
                UserName = register.Email,
                Email = register.Email,
                FullName = register.FullName,

                //EmailConfirmed = true//موقت ایمیل ها تایید بشن(برای ثبت کارابران فیک توسط خودمون)
            };

            var resultRegister = await _userManager.CreateAsync(newUser, register.Password);

            if (resultRegister.Succeeded)
            {

                //برای تایید حساب در هنگام ثبت نام واقعی
                //2 خط پایین

                //TempData["Email"] = register.Email;
                //return RedirectToAction("ConfirmEmail");

                //برای تست محیط دولوپمنت
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

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (ModelState.IsValid == false)
            {
                return View(login);
            }

            await _signInManager.SignOutAsync();

            User user = _userManager.FindByEmailAsync(login.Email).Result;

            if (user == null)
            {
                ModelState.AddModelError("", "کاربری با این ایمیل یافت نشد");

                return View(login);
            }

            var resultLogin = await _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistens, true);


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
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ConfirmEmail()
        {

            string emailUser = TempData["Email"].ToString();

            if (emailUser == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var user = await _userManager.FindByEmailAsync(emailUser);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }



            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string redirectUrl = Url.Action("VerifyEmail", "Account", new { UserId = user.Id, token = token }, Request.Scheme);

            string bodyEmail = $"لطفا برای فعالسازی حساب خود در سایت کالا مارکت بر روی لینک زیر کلیک کنید. <br/> <a href='{redirectUrl}'><h3> تایید حساب کاربری </h3></a>";

            var resultSendEmail = await _emailService.SendEmail(user.Email, bodyEmail, "تایید حساب");

            if (resultSendEmail)
            {
                return View("ConfirmEmail", user.Email);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> VerifyEmail(string UserId, string token)
        {
            if (UserId == null || token == null)
            {
                return View("FailedConfirmEmail");
            }

            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return View("FailedConfirmEmail");

            }

            var resultConfirmEmail = await _userManager.ConfirmEmailAsync(user, token);

            if (resultConfirmEmail.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return View("SuccessConfirmEmail");
            }
            else
            {
                return View("FailedConfirmEmail");
            }
        }



        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPassword)
        {

            if (ModelState.IsValid == false)
            {
                return View(forgetPassword);
            }

            var user = await _userManager.FindByEmailAsync(forgetPassword.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "کاربری با این ایمیل یافت نشد");

                return View(forgetPassword);
            }

            var resultEmailConfirm = await _userManager.IsEmailConfirmedAsync(user);

            if (resultEmailConfirm == false)//ایمیل تایید نشده
            {
                //بره ایمیلو تایید کنه

                TempData["Email"] = user.Email;

                return RedirectToAction("ConfirmEmail");
            }


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var redirectUrl = Url.Action("ResetPassword", "Account", new { UserId = user.Id, token = token }, Request.Scheme);

            string bodyEmail = $"برای بازیابی رمز عبور خود در سایت کالا مارکت بر روی لینک زیر کلیک کنید <br/> <a href={redirectUrl}> <h3> بازیابی رمز عبور </h3> </a>";

            var resultSendEmail = await _emailService.SendEmail(user.Email, bodyEmail, "بازیابی رمز عبور");
            if (resultSendEmail)
            {
                return View("SendEmailResetPassword", user.Email);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }


        public async Task<IActionResult> ResetPassword(string UserId, string token)
        {

            ResetPasswordDto resetPassword = new ResetPasswordDto
            {
                UserId = UserId,
                Token = token
            };
            return View(resetPassword);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            if (ModelState.IsValid == false)
            {
                return View(resetPassword);
            }

            var user = await _userManager.FindByIdAsync(resetPassword.UserId);

            if (user == null)
            {
                return View("FailedResetPassword");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);

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

        public async Task<IActionResult> TwoFactorLogin()
        {

            string userId = TempData["UserId"].ToString();

            bool? IsPersistans = TempData["IsPersistans"] as bool?;

            if (userId == null || IsPersistans == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            TwoFactorDto twoFactorLogin = new TwoFactorDto
            {
                IsPersistans = (bool)IsPersistans,
                Email = user.Email
            };

            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (providers.Contains("Email"))
            {
                string codeEmail = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                string bodyEmail = $"لطفا برای تکمیل ورود دو مرحله به حساب خود کد زیر را در فرم مربوطه وارد کنید <br/> <h2>{codeEmail}</h2>";
                var resultSendEmail = await _emailService.SendEmail(user.Email, bodyEmail, "ورود دو مرحله ای");

                if (resultSendEmail)
                {
                    twoFactorLogin.Provider = "Email";

                    return View(twoFactorLogin);
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> TwoFactorLogin(TwoFactorDto twoFactor)
        {
            if (ModelState.IsValid == false)
            {
                return View(twoFactor);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var resultTwoFactor = await _signInManager.TwoFactorSignInAsync(twoFactor.Provider, twoFactor.Code, twoFactor.IsPersistans, false);

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


        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            if (ModelState.IsValid == false)
            {
                return View(changePassword);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePassword.NowPassword, changePassword.NewPassword);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Profile");
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
