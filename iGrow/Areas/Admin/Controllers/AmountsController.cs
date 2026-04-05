namespace iGrow.Web.Areas.Admin.Controllers
{
    using System.Linq.Expressions;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data.Models;
    using iGrow.Web.ViewModels.Admin.Amount;
    using iGrow.Web.ViewModels;
    using iGrow.Services.Contracts;

    using static iGrow.GCommon.ApplicationConstants;
    using static iGrow.GCommon.ValidationConstants;

    [Area("Admin")]
    public class AmountsController : BaseController
    {
        private readonly IAmountService _amountService;
        private readonly ILogger<AmountsController> _logger;

        public AmountsController(IAmountService amountService, ILogger<AmountsController> logger)
        {
            this._amountService = amountService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> All(AmountAllViewModel model)
        {
            Expression<Func<Amount, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(model.SearchQuery))
            {
                string q = model.SearchQuery.Trim().ToLowerInvariant();
                filter = a => a.Name.ToLower().Contains(q);
            }

            // use service to get all and then apply paging in memory (service returns small projection)
            IEnumerable<SelectAmountId> all = await _amountService.GetAllAmountsAsync();

            if (filter != null && !string.IsNullOrWhiteSpace(model.SearchQuery))
            {
                all = all.Where(a => a.Name.ToLower().Contains(model.SearchQuery!.Trim().ToLowerInvariant()));
            }

            int total = all.Count();
            int skip = (model.PageNumber - 1) * DefaultEntitiesPerPage;

            var items = all.Skip(skip).Take(DefaultEntitiesPerPage).ToList();

            AmountAllViewModel vm = new AmountAllViewModel
            {
                SearchQuery = model.SearchQuery,
                PageNumber = model.PageNumber,
                TotalPages = (int)Math.Ceiling(total / (double)DefaultEntitiesPerPage),
                ShowingPages = model.ShowingPages,
                StartPageIndex = (model.StartPageIndex / 10) * 10,
                Amounts = items.Select(a => new AmountViewModel { Id = a.Id, Name = a.Name }).ToList()
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AmountFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AmountFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool exists = await _amountService.ItemExistsByNameAsync(model.Name);

            if (exists)
            {
                ModelState.AddModelError(nameof(model.Name), ItemExistsErrorMessage);
                return View(model);
            }

            try
            {
                await _amountService.AddAmountAsync(model.Name);
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
            var a = await _amountService.GetAmountByIdAsync(id);

            if (a == null)
            {
                return NotFound();
            }

            return View(new AmountDeleteViewModel { Id = a.Id, Name = a.Name });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _amountService.DeleteAmountAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, UnableToDeleteErrorMessage);

                var a = await _amountService.GetAmountByIdAsync(id);

                return View("Delete", new AmountDeleteViewModel { Id = a?.Id ?? id, Name = a?.Name ?? string.Empty });
            }

            return RedirectToAction(nameof(All));
        }
    }
}
