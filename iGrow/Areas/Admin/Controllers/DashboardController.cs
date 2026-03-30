namespace iGrow.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    public class DashboardController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
