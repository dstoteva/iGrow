using Microsoft.Identity.Client;

namespace iGrow.Data.Seeding.Contracts
{
    public interface IIdentitySeeder
    {
        public Task SeedRolesAsync();
        public Task SeedAdminUserAsync();
    }
}
