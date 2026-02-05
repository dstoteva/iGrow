namespace iGrow.Data
{
    using iGrow.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<MyTask> Tasks { get; set; } = null!;

        public virtual DbSet<Category> Categories { get; set; } = null!;

        public virtual DbSet<Habit> Habits { get; set; } = null!;

        public virtual DbSet<RecurringType> RecurringTypes { get; set; } = null!;

        public virtual DbSet<Amount> Amounts { get; set; } = null!;


        public override int SaveChanges()
        {
            this.ChangeTracker
                .Entries<MyTask>()
                .Where(e => e.State == EntityState.Added)
                .ToList()
                .ForEach(ee =>
                {

                    if(ee.Entity.Date == default)
                    {
                        ee.Entity.Date = DateTime.UtcNow;
                    }
                    if(ee.Entity.Priority == default)
                    {
                        ee.Entity.Priority = 1;
                    }
                    if(ee.Entity.IsCompleted == default)
                    {
                        ee.Entity.IsCompleted = false;
                    }
                });

            this.ChangeTracker
                .Entries<Habit>()
                .Where(e => e.State == EntityState.Added)
                .ToList()
                .ForEach(ee =>
                {

                    if (ee.Entity.StartDate == default)
                    {
                        ee.Entity.StartDate = DateTime.UtcNow;
                    }
                    if (ee.Entity.EndDate == default)
                    {
                        ee.Entity.EndDate = DateTime.UtcNow.AddDays(1);
                    }
                    if (ee.Entity.Priority == default)
                    {
                        ee.Entity.Priority = 1;
                    }
                    if (ee.Entity.IsCompleted == default)
                    {
                        ee.Entity.IsCompleted = false;
                    }
                });

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
