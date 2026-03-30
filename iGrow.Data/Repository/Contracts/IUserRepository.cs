namespace iGrow.Data.Repository.Contracts
{
    using iGrow.Data.Models;
    using System.Linq.Expressions;
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(Expression<Func<ApplicationUser, bool>>? filterQuery = null,
            Expression<Func<ApplicationUser, ApplicationUser>>? projectionQuery = null);

        Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser appUser);

        Task<bool> UpdateUserRoleAsync(Guid userId, string role, bool removingRole = false);

        Task<bool> DeleteUserAsync(Guid userId);
    }
}
