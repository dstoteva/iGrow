namespace iGrow.Data.Repository
{
    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;

    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserRepository(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
            : base(dbContext)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(Expression<Func<ApplicationUser, bool>>? filterQuery = null, Expression<Func<ApplicationUser, ApplicationUser>>? projectionQuery = null)
        {
            IQueryable<ApplicationUser> applicationUsers = DbContext
                .Users
                .AsNoTracking();

            if (filterQuery != null)
            {
                applicationUsers = applicationUsers
                    .Where(filterQuery);
            }

            if (projectionQuery != null)
            {
                applicationUsers = applicationUsers
                    .Select(projectionQuery);
            }

            IEnumerable<ApplicationUser> appUsers = await applicationUsers
                .OrderBy(u => u.Email)
                .ToArrayAsync();

            return appUsers;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser appUser)
        {
            IEnumerable<string> userRoles = await _userManager.GetRolesAsync(appUser);

            return userRoles;
        }

        public async Task<bool> UpdateUserRoleAsync(Guid userId, string role, bool removingRole = false)
        {
            ApplicationUser? appUser = await _userManager
                .FindByIdAsync(userId.ToString());
            if (appUser == null)
            {
                return false;
            }

            bool roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                return false;
            }

            bool alreadyInRole = await _userManager.IsInRoleAsync(appUser, role);
            if (!removingRole && alreadyInRole)
            {
                return false;
            }

            if (removingRole && !alreadyInRole)
            {
                return false;
            }

            IdentityResult roleOperationResult;
            if (removingRole)
            {
                roleOperationResult = await _userManager
                    .RemoveFromRoleAsync(appUser, role);
            }
            else
            {
                roleOperationResult = await _userManager
                    .AddToRoleAsync(appUser, role);
            }

            if (roleOperationResult != IdentityResult.Success)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            ApplicationUser? appUser = await _userManager
                .FindByIdAsync(userId.ToString());
            if (appUser == null)
            {
                return false;
            }

            IdentityResult deleteResult = await _userManager
                .DeleteAsync(appUser);
            if (deleteResult != IdentityResult.Success)
            {
                return false;
            }

            return true;
        }
    }
}
