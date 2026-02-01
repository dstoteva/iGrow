using Microsoft.AspNetCore.Mvc;

namespace iGrow.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return Json("All tasks");
        }

        public IActionResult Details(int id)
        {
            return Json(id);
        }
    }
}
