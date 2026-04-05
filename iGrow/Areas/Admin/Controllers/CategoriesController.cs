namespace iGrow.Web.Areas.Admin.Controllers
{
    using iGrow.Data.Models;
    using iGrow.Services;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels;
    using iGrow.Web.ViewModels.Admin.Category;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;
    using static iGrow.GCommon.ApplicationConstants;
    using static iGrow.GCommon.ValidationConstants;

    [Area("Admin")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            this._categoryService = categoryService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> All(CategoryAllViewModel model)
        {
            Expression<Func<Category, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(model.SearchQuery))
            {
                string q = model.SearchQuery.Trim().ToLowerInvariant();
                filter = c => c.Name.ToLower().Contains(q);
            }

            IEnumerable<SelectCategoryId> all = await _categoryService.GetAllCategoriesAsync();

            if (filter != null && !string.IsNullOrWhiteSpace(model.SearchQuery))
            {
                all = all.Where(a => a.Name.ToLower().Contains(model.SearchQuery!.Trim().ToLowerInvariant()));
            }

            int total = all.Count();
            int skip = (model.PageNumber - 1) * DefaultEntitiesPerPage;

            var items = all.Skip(skip).Take(DefaultEntitiesPerPage).ToList();

            CategoryAllViewModel vm = new CategoryAllViewModel
            {
                SearchQuery = model.SearchQuery,
                PageNumber = model.PageNumber,
                TotalPages = (int)Math.Ceiling(total / (double)DefaultEntitiesPerPage),
                ShowingPages = model.ShowingPages,
                StartPageIndex = (model.StartPageIndex / 10) * 10,
                Categories = items.Select(a => new CategoryViewModel { Id = a.Id, Name = a.Name, ImageUrl = a.ImageUrl }).ToList()
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryFormViewModel model, IFormFile? icon)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool exists = await _categoryService.ItemExistsByNameAsync(model.Name);

            if (exists)
            {
                ModelState.AddModelError(nameof(model.Name), ItemExistsErrorMessage);
                return View(model);
            }

            try
            {
                await _categoryService.AddCategoryAsync(model.Name, icon);
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
            var c = await _categoryService.GetCategoryByIdAsync(id);

            if (c == null)
            {
                return NotFound();
            }

            return View(new CategoryDeleteViewModel { Id = c.Id, Name = c.Name, ImageUrl = c.ImageUrl });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, UnableToDeleteErrorMessage);

                var c = await _categoryService.GetCategoryByIdAsync(id);

                return View("Delete", new CategoryDeleteViewModel { Id = c?.Id ?? id, Name = c?.Name ?? string.Empty, ImageUrl = c?.ImageUrl ?? String.Empty });
            }

            return RedirectToAction(nameof(All));
        }
    }
}
