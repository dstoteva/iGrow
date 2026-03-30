namespace iGrow.Services
{
    using iGrow.Data.Models;
    using iGrow.Data.Repository;
    using iGrow.Data.Repository.Contracts;
    using iGrow.GCommon.Exceptions;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels.Admin.User;

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public async Task<IEnumerable<AdminManageUserViewModel>> GetAllManageableUsersAsync(string adminUserId)
        {
            IEnumerable<ApplicationUser> appUsersNoCurrentUser = await this._userRepository
                .GetAllUsersAsync(filterQuery: u => u.Id.ToString() != adminUserId);

            IEnumerable<AdminManageUserViewModel> userAll = appUsersNoCurrentUser
                .Select(u => new AdminManageUserViewModel
                {
                    Id = u.Id,
                    Email = u.Email
                });


            foreach (AdminManageUserViewModel user in userAll)
            {
                ApplicationUser appUser = appUsersNoCurrentUser
                    .First(u => u.Id == user.Id);

                user.Roles = await this._userRepository
                    .GetUserRolesAsync(appUser);
            }

            return userAll;
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, string role)
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(role))
            {
                throw new EntityInputDataFormatException();
            }

            bool result = await this._userRepository
                .UpdateUserRoleAsync(userId, role);

            return result;
        }

        public async Task<bool> RemoveRoleFromUserAsync(Guid userId, string role)
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(role))
            {
                throw new EntityInputDataFormatException();
            }

            bool result = await this._userRepository
                .UpdateUserRoleAsync(userId, role, removingRole: true);

            return result;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new EntityInputDataFormatException();
            }

            bool result = await this._userRepository
                .DeleteUserAsync(userId);

            return result;
        }
    }
}
