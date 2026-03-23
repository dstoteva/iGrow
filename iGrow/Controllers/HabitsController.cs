namespace iGrow.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data.Models;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels.Habit;

    public class HabitsController : BaseController
    {
        private readonly IHabitService _habitService;
        private readonly ICategoryService _categoryService;
        private readonly IRecurringTypeService _recurringTypeService;
        private readonly IAmountService _amountService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HabitsController> _logger;

        public HabitsController(IHabitService habitService, UserManager<IdentityUser> userManager, ILogger<HabitsController> logger,
            ICategoryService categoryService, IRecurringTypeService recurringTypeService, IAmountService amountService)
        {
            this._habitService = habitService;
            this._categoryService = categoryService;
            this._recurringTypeService = recurringTypeService;
            this._amountService = amountService;
            this._userManager = userManager;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            string userId = this._userManager.GetUserId(this.User)!;

            IEnumerable<HabitAllViewModel> model = await this._habitService.GetAllHabitsAsync(userId);

            return View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            HabitFormViewModel model = new HabitFormViewModel();

            model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
            model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
            model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HabitFormViewModel model)
        {
            if(!this.ModelState.IsValid)
            {
                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

                return View(model);
            }

            try
            {
                string userId = this._userManager.GetUserId(this.User)!;

                await this._habitService.AddHabitAsync(model, userId);

                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "An error occurred while creating a new habit.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the new habit. Please try again later.");

                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool habitExists = await this._habitService.GetHabitByIdAsync(id) != null;

            if (!habitExists)
            {
                return NotFound();
            }

            string userId = this._userManager.GetUserId(this.User)!;
            bool isUserCreator = await this._habitService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            HabitFormViewModel model = await this._habitService.GetHabitByIdAsync(id);

            if(model == null)
            {
                return NotFound();
            }

            model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
            model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
            model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HabitFormViewModel model)
        {
            bool habitExists = await this._habitService.GetHabitByIdAsync(model.Id) != null;

            if (!habitExists)
            {
                return NotFound();
            }

            string userId = this._userManager.GetUserId(this.User)!;
            string? habitId = model.Id;

            bool isUserCreator = await this._habitService.IsUserCreatorAsync(model.Id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            if(!this.ModelState.IsValid)
            {
                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

                return View(model);
            }

            try
            {
                await this._habitService.EditHabitAsync(habitId, model);

                return RedirectToAction("Details", new { id = habitId });
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "An error occurred while editing the habit.");
                ModelState.AddModelError(string.Empty, "An error occurred while editing the habit. Please try again later.");

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            bool habitExists = await this._habitService.GetHabitByIdAsync(id) != null;

            if (!habitExists)
            {
                return NotFound();
            }

            string userId = this._userManager.GetUserId(this.User)!;
            bool isUserCreator = await this._habitService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            HabitDetailsViewModel model = await this._habitService.GetHabitDetailsAsync(id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool habitExists = await this._habitService.GetHabitByIdAsync(id) != null;

            if (!habitExists)
            {
                return NotFound();
            }

            string userId = this._userManager.GetUserId(this.User)!;
            bool isUserCreator = await this._habitService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            HabitDeleteViewModel model = await this._habitService.GetHabitToBeDeletedAsync(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            bool habitExists = await this._habitService.GetHabitByIdAsync(id) != null;

            if (!habitExists)
            {
                return NotFound();
            }

            string userId = this._userManager.GetUserId(this.User)!;
            bool isUserCreator = await this._habitService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            try
            {
                await this._habitService.DeleteHabitAsync(id);

                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "An error occurred while editing the habit.");
                ModelState.AddModelError(string.Empty, "An error occurred while editing the habit. Please try again later.");

                return RedirectToAction("Details", new { id });
            }
        }
    }
}
