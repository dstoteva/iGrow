namespace iGrow.Web.Areas.Admin.Controllers
{
    using iGrow.Data.Models;
    using iGrow.Data.Seeding;
    using iGrow.GCommon.Exceptions;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels.Admin.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using static iGrow.GCommon.ApplicationConstants;
    using static iGrow.GCommon.ValidationConstants;

    public class UserManagementController : BaseController
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            this._userService = userService;
            this._userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = this._userManager.GetUserId(this.User)!;

            IEnumerable<AdminManageUserViewModel> users = await this._userService
                .GetAllManageableUsersAsync(userId);

            ViewData["AllRoles"] = IdentitySeeder.ApplicationRoles;

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(Guid userId, string role)
        {
            try
            {
                bool assignRoleResult = await this._userService
                    .AssignRoleToUserAsync(userId, role);

                if (!assignRoleResult)
                {
                    TempData[ErrorTempDataKey] = string.Format(UserRoleAssignmentIdentityErrorMessage, role);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (EntityInputDataFormatException eidfe)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                TempData[ErrorTempDataKey] = string.Format(UserRoleAssignmentFailureMessage, role, "assigning");

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(Guid userId, string role)
        {
            try
            {
                bool removeRoleResult = await this._userService
                    .RemoveRoleFromUserAsync(userId, role);
                if (!removeRoleResult)
                {
                    TempData[ErrorTempDataKey] = string.Format(UserRoleRemoveIdentityErrorMessage, role);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (EntityInputDataFormatException eidfe)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                TempData[ErrorTempDataKey] = string.Format(UserRoleAssignmentFailureMessage, role, "removing");

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            try
            {
                bool deleteResult = await this._userService
                    .DeleteUserAsync(userId);
                if (!deleteResult)
                {
                    TempData[ErrorTempDataKey] = UserDeleteNotExistMessage;

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (EntityInputDataFormatException eidfe)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                TempData[ErrorTempDataKey] = UserDeleteFailureErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
