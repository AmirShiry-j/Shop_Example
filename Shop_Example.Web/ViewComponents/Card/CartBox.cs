using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Services.Card;
using Shop_Example.Entities.Models;
using Shop_Example.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.ViewComponents.Card
{
    [ViewComponent]
    public class CartBox : ViewComponent
    {
        private readonly ICartService _cartService;
        private readonly ICookiesManeger _cookiesManeger;
        private readonly UserManager<User> _userManager;
        public CartBox(ICartService cartService, UserManager<User> userManager)
        {
            _cartService = cartService;
            _cookiesManeger = new CookiesManeger();
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = null;
            if (UserClaimsPrincipal.Identity.IsAuthenticated)
            {
                userId = _userManager.GetUserId(UserClaimsPrincipal);
            }
            return View("/Views/Cart/ViewComponents/CartBox.cshtml", _cartService.GetMyCart(_cookiesManeger.GetBrowserId(HttpContext), userId).Data);
        }
    }
}
