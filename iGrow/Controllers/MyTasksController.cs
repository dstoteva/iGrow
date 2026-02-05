namespace iGrow.Controllers
{
    using iGrow.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    public class MyTasksController : Controller
    {
        private readonly IMyTaskService _taskService;
        public MyTasksController(IMyTaskService taskService)
        {
            this._taskService = taskService;
        }
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
