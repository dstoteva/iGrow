namespace iGrow.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    using iGrow.Data.Seeding.Contracts;

    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UseRolesSeeder(this IApplicationBuilder applicationBuilder)
        {
            using IServiceScope scope = applicationBuilder.ApplicationServices.CreateScope();
            IIdentitySeeder identitySeeder = scope.ServiceProvider.GetRequiredService<IIdentitySeeder>();

            identitySeeder.SeedRolesAsync().GetAwaiter().GetResult();

            return applicationBuilder;
        }

        public static IApplicationBuilder UseAdminSeeder(this IApplicationBuilder applicationBuilder)
        {
            using IServiceScope scope = applicationBuilder.ApplicationServices.CreateScope();
            IIdentitySeeder identitySeeder = scope.ServiceProvider.GetRequiredService<IIdentitySeeder>();

            identitySeeder.SeedAdminUserAsync().GetAwaiter().GetResult();

            return applicationBuilder;
        }
    }
}
