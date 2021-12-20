using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop_Example.DataLayer.Services.Card;
using Shop_Example.Entities.Models;
using Shop_Example.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Controllers
{
    [Route("/{Controller}/{Action}/")]
    public class CartController : Controller
    {
        private readonly ICookiesManeger _cookiesManeger;
        private readonly ICartService _cartService;
        private readonly UserManager<User> _userManager;
        public CartController(UserManager<User> userManager, ICookiesManeger cookiesManeger, ICartService cartService)
        {
            _userManager = userManager;
            _cartService = cartService;
            _cookiesManeger = cookiesManeger;
        }
        public async Task<IActionResult> Index()
        {

            string userId = null;

            if (User.Identity.IsAuthenticated)
            {
                userId = _userManager.GetUserId(User);
            }

            var resultGetLst = _cartService.GetMyCart(_cookiesManeger.GetBrowserId(HttpContext), userId);

            return View(resultGetLst.Data);
        }

        [Route("{ProductId}")]
        public async Task<bool> AddToCart(int ProductId)
        {

            var resultAdd = _cartService.AddToCart(ProductId, _cookiesManeger.GetBrowserId(HttpContext));

            return resultAdd.IsSuccess;

        }

        [Route("{CartItemId}")]
        public async Task<IActionResult> Add(long CartItemId)
        {
            var result = _cartService.Add(CartItemId);

            return RedirectToAction(nameof(Index));
        }

        [Route("{CartItemId}")]
        public async Task<IActionResult> LowOff(long CartItemId)
        {
            var result = _cartService.LowOff(CartItemId);

            return RedirectToAction(nameof(Index));
        }

        [Route("{ProductId}")]
        public async Task<bool> RemoveToCart(int ProductId)
        {
            var result = _cartService.RemoveFromCart(ProductId, _cookiesManeger.GetBrowserId(HttpContext));

            return result.IsSuccess;
        }
    }
}
