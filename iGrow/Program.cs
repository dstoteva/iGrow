namespace iGrow
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data;
    using iGrow.Services;
    using iGrow.Web.Infrastructure.Extensions;
    using iGrow.Data.Repository;
    using iGrow.Data.Models;
    using iGrow.Data.Seeding;
    using iGrow.Data.Seeding.Contracts;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddTransient<IIdentitySeeder, IdentitySeeder>();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                ConfigureIdentity(options, builder.Configuration);
            })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.RegisterRepositories(typeof(MyTaskRepository));
            builder.Services.RegisterRepositories(typeof(HabitRepository));
            builder.Services.RegisterRepositories(typeof(CategoryRepository));
            builder.Services.RegisterRepositories(typeof(RecurringTypeRepository));
            builder.Services.RegisterRepositories(typeof(AmountRepository));

            builder.Services.RegisterUserServices(typeof(MyTaskService));
            builder.Services.RegisterUserServices(typeof(HabitService));
            builder.Services.RegisterUserServices(typeof(CategoryService));
            builder.Services.RegisterUserServices(typeof(RecurringTypeService));
            builder.Services.RegisterUserServices(typeof(AmountService));

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRolesSeeder();
            app.UseAdminSeeder();

            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "adminArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }

        private static void ConfigureIdentity(IdentityOptions options, ConfigurationManager configurationManager)
        {
            options.SignIn.RequireConfirmedAccount = configurationManager.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            options.SignIn.RequireConfirmedEmail = configurationManager.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
            options.SignIn.RequireConfirmedPhoneNumber = configurationManager.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");

            options.User.RequireUniqueEmail = configurationManager.GetValue<bool>("Identity:User:RequireUniqueEmail");

            options.Lockout.MaxFailedAccessAttempts = configurationManager.GetValue<int>("Identity:Lockout:MaxFailedAccessAttempts");
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(configurationManager.GetValue<int>("Identity:Lockout:DefaultLockoutTimeSpanMin"));

            options.Password.RequireDigit = configurationManager.GetValue<bool>("Identity:Password:RequireDigit");
            options.Password.RequireLowercase = configurationManager.GetValue<bool>("Identity:Password:RequireLowercase");
            options.Password.RequireUppercase = configurationManager.GetValue<bool>("Identity:Password:RequireUppercase");
            options.Password.RequireNonAlphanumeric = configurationManager.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
            options.Password.RequiredLength = configurationManager.GetValue<int>("Identity:Password:RequiredLength");
            options.Password.RequiredUniqueChars= configurationManager.GetValue<int>("Identity:Password:RequiredUniqueChars");
        }
    }
}
