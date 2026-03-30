namespace iGrow.Data.Seeding
{
    using Microsoft.AspNetCore.Identity;

    using iGrow.Data.Seeding.Contracts;
    using iGrow.Data.Models;

    using static iGrow.GCommon.ValidationConstants;
    using Microsoft.Extensions.Configuration;

    public class IdentitySeeder : IIdentitySeeder
    {
        public static string[] ApplicationRoles = new[]
        {
            "Admin",
            "User"
        };

        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public IdentitySeeder(RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task SeedRolesAsync()
        {
            foreach (string role in ApplicationRoles)
            {
                bool roleExists = await _roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    IdentityRole<Guid> identityRole = new IdentityRole<Guid>(role);

                    IdentityResult identityResult = await _roleManager.CreateAsync(identityRole);

                    if (!identityResult.Succeeded)
                    {
                        throw new InvalidOperationException(RoleSeedingFailure);
                    }
                }
            }
        }

        public async Task SeedAdminUserAsync()
        {
            string adminEmail = this._configuration["UserSeed:AdminAccount:Email"] ?? throw new InvalidOperationException(AdminUserSeedingNotFound);
            string adminPassword = this._configuration["UserSeed:AdminAccount:Password"] ?? throw new InvalidOperationException(AdminUserSeedingPasswordNotFound);

            ApplicationUser? adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if(adminUser == null)
            {
                ApplicationUser newAdminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                IdentityResult identityResult = await _userManager.CreateAsync(newAdminUser, adminPassword);

                if (!identityResult.Succeeded)
                {
                    throw new InvalidOperationException(AdminUserSeedingException);
                }

                bool isInRole = await _userManager.IsInRoleAsync(newAdminUser, ApplicationRoles[0]);

                if (!isInRole)
                {
                    IdentityResult result = await _userManager.AddToRoleAsync(newAdminUser, ApplicationRoles[0]);

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(AdminUserSeedingException);
                    }
                }
            }
        }
    }
}
