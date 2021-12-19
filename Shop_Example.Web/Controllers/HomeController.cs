using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Example.Web.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index()
        {
         

            return View();
        }

        [HttpGet("/Error")]
        public async Task<IActionResult> Error()
        {
            return View();
        }
    }
}
