namespace iGrow.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using iGrow.Services.Contracts;
    using iGrow.Web.Controllers;
    using iGrow.Web.ViewModels.MyTask;

    public class MyTasksController : BaseController
    {
        private readonly IMyTaskService _taskService;
        private readonly ICategoryService _categoryService;
        private readonly IRecurringTypeService _recurringTypeService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<MyTasksController> _logger;
        public MyTasksController(IMyTaskService taskService, UserManager<IdentityUser> userManager, ILogger<MyTasksController> logger,
            ICategoryService categoryService, IRecurringTypeService recurringTypeService)
        {
            this._taskService = taskService;
            this._categoryService = categoryService;
            this._recurringTypeService = recurringTypeService;
            this._userManager = userManager;
            this._logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            string? userId = this._userManager.GetUserId(this.User);

            IEnumerable<MyTaskAllViewModel> model = await this._taskService.GetAllTasksAsync(userId);

            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            MyTaskFormViewModel model = new MyTaskFormViewModel();

            model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
            model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MyTaskFormViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();

                return View(model);
            }

            try
            {
                model.UserId = this._userManager.GetUserId(this.User);

                await this._taskService.AddTaskAsync(model);

                return RedirectToAction("All", "MyTasks");
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "An error occurred while creating a new task.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the new task. Please try again later.");
                
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            MyTaskFormViewModel model = await this._taskService.GetTaskById(id);

            model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
            model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, MyTaskFormViewModel model)
        {
            bool taskExists = await this._taskService.GetTaskById(id) != null;

            if (!taskExists)
            {
                return NotFound();
            }
            if(!this.ModelState.IsValid)
            {
                return View(model);
            }

            model.UserId = this._userManager.GetUserId(this.User);

            await this._taskService.EditTaskAsync(id, model);

            return RedirectToAction("All", "MyTasks");
        }

        public async Task<IActionResult> Details(string id)
        {
            bool taskExists = await this._taskService.GetTaskById(id) != null;

            if (!taskExists)
            {
                return NotFound();
            }

            MyTaskDetailsViewModel model = await this._taskService.GetTaskDetails(id);

            return View(model);
        }
    }
}
