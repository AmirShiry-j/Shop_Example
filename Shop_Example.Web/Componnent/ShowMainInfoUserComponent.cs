using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.Dtoes.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Shop_Example.Entities.Models;

namespace Shop_Example.Web.Componnent
{
    public class ShowMainInfoUserComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        public ShowMainInfoUserComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Run(() =>
            {
                var user = _userManager.GetUserAsync(UserClaimsPrincipal).Result;

                var model = new InfoMiniPanelDto
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsConfirmedPhoneNumber = user.PhoneNumberConfirmed,
                    ImageProfileName = user.ImageProfileName
                };

                return View("/Views/Component/ShowMainInfoUser.cshtml", model);
            });
        }

    }
}
