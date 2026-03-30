namespace iGrow.Services.Contracts
{
    using iGrow.Web.ViewModels.Admin.User;
    public interface IUserService
    {
        Task<IEnumerable<AdminManageUserViewModel>> GetAllManageableUsersAsync(string adminUserId);

        Task<bool> AssignRoleToUserAsync(Guid userId, string role);

        Task<bool> RemoveRoleFromUserAsync(Guid userId, string role);

        Task<bool> DeleteUserAsync(Guid userId);
    }
}
