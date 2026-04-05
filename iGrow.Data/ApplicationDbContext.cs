namespace iGrow.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
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
            builder.Entity<MyTask>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MyTask>()
                .HasOne(t => t.RecurringType)
                .WithMany(rt => rt.Tasks)
                .HasForeignKey(t => t.RecurringTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MyTask>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Habit>()
                .HasOne(h => h.Category)
                .WithMany(c => c.Habits)
                .HasForeignKey(h => h.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Habit>()
                .HasOne(h => h.RecurringType)
                .WithMany(rt => rt.Habits)
                .HasForeignKey(h => h.RecurringTypeId)
                .OnDelete(DeleteBehavior.Cascade);

             builder.Entity<Habit>()
                .HasOne(h => h.Amount)
                .WithMany(a => a.Habits)
                .HasForeignKey(h => h.AmountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Habit>()
                .HasOne(h => h.User)
                .WithMany(u => u.Habits)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Quit a bad habit", ImageUrl = "/images/categories/quit.svg" },
                new Category { Id = 2, Name = "Art", ImageUrl = "/images/categories/art.svg" },
                new Category { Id = 3, Name = "Task", ImageUrl = "/images/categories/task.svg" },
                new Category { Id = 4, Name = "Meditation", ImageUrl = "/images/categories/meditation.svg" },
                new Category { Id = 5, Name = "Study", ImageUrl = "/images/categories/study.svg" },
                new Category { Id = 6, Name = "Sports", ImageUrl = "/images/categories/sports.svg" },
                new Category { Id = 7, Name = "Entertainment", ImageUrl = "/images/categories/entertainment.svg" },
                new Category { Id = 8, Name = "Social", ImageUrl = "/images/categories/social.svg" },
                new Category { Id = 9, Name = "Finance", ImageUrl = "/images/categories/finance.svg" },
                new Category { Id = 10, Name = "Health", ImageUrl = "/images/categories/health.svg" },
                new Category { Id = 11, Name = "Work", ImageUrl = "/images/categories/work.svg" },
                new Category { Id = 12, Name = "Nutrition", ImageUrl = "/images/categories/nutrition.svg" },
                new Category { Id = 13, Name = "Home", ImageUrl = "/images/categories/home.svg" },
                new Category { Id = 14, Name = "Outdoor", ImageUrl = "/images/categories/outdoor.svg" },
                new Category { Id = 100, Name = "Other", ImageUrl = "/images/categories/other.svg" }
            );

            builder.Entity<RecurringType>().HasData(
                new RecurringType { Id = 1, Name = "Daily" },
                new RecurringType { Id = 2, Name = "Weekly" },
                new RecurringType { Id = 3, Name = "Monthly" },
                new RecurringType { Id = 4, Name = "Yearly" },
                new RecurringType { Id = 5, Name = "None" }
            );

            builder.Entity<Amount>().HasData(
                new Amount { Id = 1, Name = "At least" },
                new Amount { Id = 2, Name = "Less than" },
                new Amount { Id = 3, Name = "Exactly" },
                new Amount { Id = 4, Name = "Any value" }
            );

            base.OnModelCreating(builder);
        }
    }
}
