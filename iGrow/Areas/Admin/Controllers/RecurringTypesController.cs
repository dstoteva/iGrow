namespace iGrow.Web.Areas.Admin.Controllers
{
    using iGrow.Data.Models;
    using iGrow.Services;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels;
    using iGrow.Web.ViewModels.Admin.RecurringType;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;
    using static iGrow.GCommon.ApplicationConstants;
    using static iGrow.GCommon.ValidationConstants;

    public class RecurringTypesController : BaseController
    {
        private readonly IRecurringTypeService _recurringTypeService;
        private readonly ILogger<RecurringTypesController> _logger;

        public RecurringTypesController(IRecurringTypeService recurringTypeService, ILogger<RecurringTypesController> logger)
        {
            this._recurringTypeService = recurringTypeService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> All(RecurringTypeAllViewModel model)
        {
            Expression<Func<RecurringType, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(model.SearchQuery))
            {
                string q = model.SearchQuery.Trim().ToLowerInvariant();
                filter = rt => rt.Name.ToLower().Contains(q);
            }

            IEnumerable<SelectRecurringTypeId> all = await _recurringTypeService.GetAllRecurringTypesAsync();

            if (filter != null)
            {
                all = all.Where(a => a.Name.ToLower().Contains(model.SearchQuery!.Trim().ToLowerInvariant()));
            }

            int total = all.Count();
            int skip = (model.PageNumber - 1) * DefaultEntitiesPerPage;

            var items = all.Skip(skip).Take(DefaultEntitiesPerPage).ToList();

            RecurringTypeAllViewModel vm = new RecurringTypeAllViewModel
            {
                SearchQuery = model.SearchQuery,
                PageNumber = model.PageNumber,
                TotalPages = (int)Math.Ceiling(total / (double)DefaultEntitiesPerPage),
                ShowingPages = model.ShowingPages,
                StartPageIndex = (model.StartPageIndex / 10) * 10,
                RecurringTypes = items.Select(a => new RecurringTypeViewModel { Id = a.Id, Name = a.Name }).ToList()
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new RecurringTypeFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecurringTypeFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool exists = await _recurringTypeService.ItemExistsByNameAsync(model.Name);

            if (exists)
            {
                ModelState.AddModelError(nameof(model.Name), ItemExistsErrorMessage);
                return View(model);
            }

            try
            {
                await _recurringTypeService.AddRecurringTypeAsync(model.Name);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(model.Name), ex.Message);
                return View(model);
            }

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var rt = await _recurringTypeService.GetRecurringTypeByIdAsync(id);

            if (rt == null)
            {
                return NotFound();
            }

            return View(new RecurringTypeDeleteViewModel { Id = rt.Id, Name = rt.Name });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _recurringTypeService.DeleteRecurringTypeAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, UnableToDeleteErrorMessage);

                var rt = await _recurringTypeService.GetRecurringTypeByIdAsync(id);

                return View("Delete", new RecurringTypeDeleteViewModel { Id = rt?.Id ?? id, Name = rt?.Name ?? string.Empty });
            }

            return RedirectToAction(nameof(All));
        }
    }
}
