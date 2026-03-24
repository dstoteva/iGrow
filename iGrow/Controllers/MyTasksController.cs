namespace iGrow.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using iGrow.Services.Contracts;
    using iGrow.Web.Controllers;
    using iGrow.Web.ViewModels.MyTask;
    using iGrow.GCommon.Exceptions;

    using static iGrow.GCommon.ValidationConstants;

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
            string userId = this._userManager.GetUserId(this.User)!;

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
                string userId = this._userManager.GetUserId(this.User)!;

                await this._taskService.AddTaskAsync(model, userId);
            }
            catch (EntityPersistFailureException e)
            {
                this._logger.LogError(e, TaskPersistenceErrorMessage);
                ModelState.AddModelError(string.Empty, TaskPersistenceErrorMessage);

                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();

                return View(model);
            }
            catch(Exception e)
            {
                this._logger.LogError(e, UnexpectedErrorMessage);
                ModelState.AddModelError(string.Empty, UnexpectedErrorMessage);

                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();

                return View(model);
            }

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            string userId = this._userManager.GetUserId(this.User)!;
            bool isUserCreator = await this._taskService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            MyTaskFormViewModel model = await this._taskService.GetTaskByIdAsync(id);

            if (model == null)
            {
                throw new EntityNotFoundException();
            }

            model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
            model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
            model.Id = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MyTaskFormViewModel model)
        {
            string userId = this._userManager.GetUserId(this.User)!;
            string? taskId = model.Id;

            bool isUserCreator = await this._taskService.IsUserCreatorAsync(taskId, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            if (!this.ModelState.IsValid)
            {
                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();

                return View(model);
            }

            try
            {
                bool isEditSuccessful = await this._taskService.EditTaskAsync(taskId, model);

                if (!isEditSuccessful)
                {
                    throw new EntityPersistFailureException();
                }

                return RedirectToAction("Details", new { id = taskId });
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException e)
            {
                this._logger.LogError(e, TaskPersistenceErrorMessage);
                ModelState.AddModelError(string.Empty, TaskPersistenceErrorMessage);

                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();

                return View(model);
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "An error occurred while editing the task.");
                ModelState.AddModelError(string.Empty, "An error occurred while editing the task. Please try again later.");

                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            string userId = this._userManager.GetUserId(this.User)!;
            bool isUserCreator = await this._taskService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            try
            {
                MyTaskDetailsViewModel model = await this._taskService.GetTaskDetailsAsync(id);

                return View(model);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "An error occurred while getting the task.");
                ModelState.AddModelError(string.Empty, "An error occurred while getting the task. Please try again later.");

                return RedirectToAction("Details", new { id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            string userId = this._userManager.GetUserId(this.User)!;
            bool isUserCreator = await this._taskService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            try
            {
                MyTaskDeleteViewModel model = await this._taskService.GetTaskToBeDeletedAsync(id);
                return View(model);
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
             catch (Exception e)
            {
                this._logger.LogError(e, "An error occurred while preparing the delete confirmation.");
                ModelState.AddModelError(string.Empty, "An error occurred while preparing the delete confirmation. Please try again later.");

                return RedirectToAction("Details", new { id });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            string userId = this._userManager.GetUserId(this.User)!;
            bool isUserCreator = await this._taskService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            try
            {
                await this._taskService.SoftDeleteTaskAsync(id);

                return RedirectToAction("All");
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException e)
            {
                this._logger.LogError(e, TaskPersistenceErrorMessage);
                ModelState.AddModelError(string.Empty, TaskPersistenceErrorMessage);

                return RedirectToAction("Delete", new { id });
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "An error occurred while deleting the task.");
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the task. Please try again later.");

                return RedirectToAction("Details", new { id });
            }
        }
    }
}
