namespace iGrow.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using iGrow.Web.ViewModels;
    using iGrow.Web.Controllers;

    public class HomeController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(UserManager<IdentityUser> userManager)
        {
            this._userManager = userManager;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            string? userId = this._userManager.GetUserId(this.User);
            if (!String.IsNullOrEmpty(userId))
            {
                return RedirectToAction("All", "MyTasks");
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
