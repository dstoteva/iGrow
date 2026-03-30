namespace iGrow.Controllers
{
    using iGrow.Data.Models;
    using iGrow.Web.Controllers;
    using iGrow.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Home/Error")]
        [Route("Home/Error/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if(statusCode == StatusCodes.Status400BadRequest)
            {
                return View("BadRequest");
            }
            if(statusCode == StatusCodes.Status404NotFound)
            {
                return View("NotFound");
            }
            if(statusCode == StatusCodes.Status500InternalServerError)
            {
                return View("ServerError");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
