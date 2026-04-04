namespace iGrow.Data.Repository
{
    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;

    public class HabitRepository : BaseRepository, IHabitRepository
    {
        public HabitRepository(ApplicationDbContext dbContext)
            :base(dbContext)
        {
                
        }
        public async Task<IEnumerable<Habit>> GetAllHabitsNoTrackingByUserIdWithCategoryAndRecurringTypeAndAmountAsync(string userId, Expression<Func<Habit, bool>>? filterQuery, int? skipCnt, int? takeCnt)
        {
            IQueryable<Habit> habitsFetchQuery = this.DbContext!.Habits
                .AsNoTracking()
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.Title);

            if (filterQuery != null)
            {
                habitsFetchQuery = habitsFetchQuery
                    .Where(filterQuery)
                    .AsQueryable();
            }

            habitsFetchQuery = habitsFetchQuery
                .Where(t => t.UserId == Guid.Parse(userId))
                .Where(t => !t.IsDeleted)
                .Include(t => t.Category)
                .Include(t => t.RecurringType)
                .Include(t => t.Amount)
                .Include(t => t.User)
                .AsQueryable();

            if (skipCnt.HasValue && skipCnt > 0)
            {
                habitsFetchQuery = habitsFetchQuery
                    .Skip(skipCnt.Value)
                    .AsQueryable();
            }
            if (takeCnt.HasValue && takeCnt > 0)
            {
                habitsFetchQuery = habitsFetchQuery
                    .Take(takeCnt.Value)
                    .AsQueryable();
            }

            return await habitsFetchQuery.ToArrayAsync();
        }

        public async Task<IEnumerable<Habit>> GetAllHabitsAsync(string userId)
        {
            return await DbContext!.Habits
                .AsNoTracking()
                .Where(t => t.UserId == Guid.Parse(userId))
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.Title)
                .ToArrayAsync();
        }

        public async Task<Habit?> GetHabitByIdAsync(string id)
        {
            return await DbContext!.Habits
                .Where(t => t.Id.ToString() == id)
                .Where(t => !t.IsDeleted)
                .Include(t => t.RecurringType)
                .Include(t => t.Category)
                .Include(t => t.Amount)
                .Include(t => t.User)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AddHabitAsync(Habit habit)
        {
            await DbContext!.Habits.AddAsync(habit);

            int resutCount = await SaveChangesAsync();

            return resutCount == 1;
        }

        public async Task<bool> EditHabitAsync(Habit habit)
        {
            DbContext!.Habits.Update(habit);

            int resutCount = await SaveChangesAsync();

            return resutCount == 1;
        }

        public async Task<bool> SoftDeleteHabitAsync(Habit habit)
        {
            habit.IsDeleted = true;

            DbContext!.Habits.Update(habit);

            int resutCount = await SaveChangesAsync();

            return resutCount == 1;
        }

        public async Task<bool> HardDeleteHabitAsync(Habit habit)
        {
            DbContext!.Habits.Remove(habit);

            int resutCount = await SaveChangesAsync();

            return resutCount == 1;
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await DbContext!.Habits
                .AnyAsync(t => t.Id == Guid.Parse(id) && !t.IsDeleted);
        }
        public async Task<int> CountAsync(string userId, Expression<Func<Habit, bool>>? filterQuery)
        {
            IQueryable<Habit> habitsFetchQuery = DbContext!.Habits
                .AsNoTracking()
                .Where(h => h.UserId.ToString() == userId)
                .Where(h => !h.IsDeleted);

            if (filterQuery != null)
            {
                habitsFetchQuery = habitsFetchQuery
                    .Where(filterQuery)
                    .AsQueryable();
            }

            return await habitsFetchQuery.CountAsync();
        }
    }
}
