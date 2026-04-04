namespace iGrow.Web.Controllers
{
    using iGrow.Data.Models;
    using iGrow.GCommon.Exceptions;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels.Habit;
    using iGrow.Web.ViewModels.MyTask;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using static iGrow.GCommon.ValidationConstants;
    using static iGrow.GCommon.ApplicationConstants;

    public class HabitsController : BaseController
    {
        private readonly IHabitService _habitService;
        private readonly ICategoryService _categoryService;
        private readonly IRecurringTypeService _recurringTypeService;
        private readonly IAmountService _amountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HabitsController> _logger;

        public HabitsController(IHabitService habitService, UserManager<ApplicationUser> userManager, ILogger<HabitsController> logger,
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
        public async Task<IActionResult> All(HabitAllViewModel model)
        {
            string userId = this._userManager.GetUserId(this.User)!;

            IEnumerable<HabitViewModel> habitViewModel = await this._habitService.GetAllHabitsAsync(userId, model.SearchQuery, model.PageNumber);

            int habitsCount = await this._habitService.GetHabitsCountAsync(userId, model.SearchQuery);

            HabitAllViewModel allHabitsViewModel = new HabitAllViewModel()
            {
                SearchQuery = model.SearchQuery,
                PageNumber = model.PageNumber,
                TotalPages = (int)Math.Ceiling(habitsCount / (double)DefaultEntitiesPerPage),
                ShowingPages = model.ShowingPages,
                StartPageIndex = (model.StartPageIndex / 10) * 10,
                Habits = habitViewModel.ToArray(),
            };

            if (allHabitsViewModel.PageNumber > allHabitsViewModel.TotalPages && allHabitsViewModel.TotalPages != 0)
            {
                return RedirectToAction(nameof(All), new HabitAllViewModel
                {
                    SearchQuery = model.SearchQuery,
                    PageNumber = allHabitsViewModel.TotalPages,
                    ShowingPages = model.ShowingPages
                });
            }

            return View(allHabitsViewModel);
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
        public async Task<IActionResult> Create(HabitFormViewModel model)
        {
            if(!this.ModelState.IsValid)
            {
                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

                return View(model);
            }

            DateTime startDate = DateTime.Parse(model.StartDate);
            DateTime endDate = DateTime.Parse(model.EndDate);


            if (DateTime.Compare(startDate, endDate) >= 0)
            {
                ModelState.AddModelError(nameof(model.StartDate), StartDateMustBeBeforeEndDate);

                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

                return View(model);
            }

            try
            {
                string userId = this._userManager.GetUserId(this.User)!;

                await this._habitService.AddHabitAsync(model, userId);
            }
            catch (EntityPersistFailureException e)
            {
                this._logger.LogError(e, HabitPersistenceErrorMessage);
                ModelState.AddModelError(string.Empty, HabitPersistenceErrorMessage);

                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

                return View(model);
            }
            catch (Exception e)
            {
                this._logger.LogError(e, UnexpectedErrorMessage);
                ModelState.AddModelError(string.Empty, UnexpectedErrorMessage);

                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

                return View(model);
            }
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            string userId = this._userManager.GetUserId(this.User)!;
            bool isUserCreator = await this._habitService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            HabitFormViewModel model = await this._habitService.GetHabitByIdAsync(id);

            if(model == null)
            {
                throw new EntityNotFoundException();
            }

            model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
            model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
            model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(HabitFormViewModel model)
        {
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

            DateTime startDate = DateTime.Parse(model.StartDate);
            DateTime endDate = DateTime.Parse(model.EndDate);


            if (DateTime.Compare(startDate, endDate) >= 0)
            {
                ModelState.AddModelError(nameof(model.StartDate), StartDateMustBeBeforeEndDate);

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
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException e)
            {
                this._logger.LogError(e, HabitPersistenceErrorMessage);
                ModelState.AddModelError(string.Empty, HabitPersistenceErrorMessage);

                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

                return View(model);
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "An error occurred while editing the habit.");
                ModelState.AddModelError(string.Empty, "An error occurred while editing the habit. Please try again later.");

                model.RecurringTypes = this._recurringTypeService.GetAllRecurringTypesAsync().GetAwaiter().GetResult();
                model.Categories = this._categoryService.GetAllCategoriesAsync().GetAwaiter().GetResult();
                model.Amounts = this._amountService.GetAllAmountsAsync().GetAwaiter().GetResult();

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            string userId = this._userManager.GetUserId(this.User)!;
            bool isUserCreator = await this._habitService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            try
            {
                HabitDetailsViewModel model = await this._habitService.GetHabitDetailsAsync(id);

                return View(model);
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                this._logger.LogError(e, "An error occurred while getting the habit.");
                ModelState.AddModelError(string.Empty, "An error occurred while getting the habit. Please try again later.");

                return RedirectToAction("Details", new { id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            string userId = this._userManager.GetUserId(this.User)!;
            bool isUserCreator = await this._habitService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            try
            {
                HabitDeleteViewModel model = await this._habitService.GetHabitToBeDeletedAsync(id);
                return View(model);
            }
            catch (EntityNotFoundException)
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
            bool isUserCreator = await this._habitService.IsUserCreatorAsync(id, userId);

            if (!isUserCreator)
            {
                return Forbid();
            }

            try
            {
                await this._habitService.SoftDeleteHabitAsync(id);

                return RedirectToAction(nameof(All));
            }
            catch(EntityNotFoundException)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException e)
            {
                this._logger.LogError(e, HabitPersistenceErrorMessage);
                ModelState.AddModelError(string.Empty, HabitPersistenceErrorMessage);

                return RedirectToAction("Delete", new { id });
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
