namespace iGrow.Data.Repository
{
    using System.Linq.Expressions;

    using Microsoft.EntityFrameworkCore;

    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;

    public class MyTaskRepository : BaseRepository, IMyTaskRepository
    {
        public MyTaskRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
                
        }
        public async Task<IEnumerable<MyTask>> GetAllTasksNoTrackingByUserIdWithCategoryAndRecurringTypeAsync(string userId, Expression<Func<MyTask, bool>>? filterQuery = null, int? skipCnt = null, int? takeCnt = null)
        {
            IQueryable<MyTask> tasksFetchQuery = this.DbContext!.Tasks
                .AsNoTracking()
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.Title);

            if (filterQuery != null)
            {
                tasksFetchQuery = tasksFetchQuery
                    .Where(filterQuery)
                    .AsQueryable();
            }

            tasksFetchQuery = tasksFetchQuery
                .Where(t => t.UserId == Guid.Parse(userId))
                .Where(t => !t.IsDeleted)
                .Include(t => t.Category)
                .Include(t => t.RecurringType)
                .Include(t => t.User)
                .AsQueryable();

            if (skipCnt.HasValue && skipCnt > 0)
            {
                tasksFetchQuery = tasksFetchQuery
                    .Skip(skipCnt.Value)
                    .AsQueryable();
            }
            if (takeCnt.HasValue && takeCnt > 0)
            {
                tasksFetchQuery = tasksFetchQuery
                    .Take(takeCnt.Value)
                    .AsQueryable();
            }

            // inside the repository, before executing:
            var sql = tasksFetchQuery.ToQueryString();
            System.Diagnostics.Debug.WriteLine(sql);

            return await tasksFetchQuery.ToArrayAsync();
        }

        public async Task<IEnumerable<MyTask>> GetAllTasksAsync(string userId)
        {
            return await DbContext!.Tasks
                .AsNoTracking()
                .Where(t => t.UserId == Guid.Parse(userId))
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.Title)
                .ToArrayAsync();
        }

        public async Task<MyTask?> GetTaskByIdAsync(string id)
        {
            return await DbContext!.Tasks
                .Where(t => t.Id.ToString() == id)
                .Where(t => !t.IsDeleted)
                .Include(t => t.RecurringType)
                .Include(t => t.Category)
                .Include(t => t.User)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AddTaskAsync(MyTask task)
        {
            await DbContext!.Tasks.AddAsync(task);

            int resutCount = await SaveChangesAsync();

            return resutCount == 1;
        }

        public async Task<bool> EditTaskAsync(MyTask task)
        {
            DbContext!.Tasks.Update(task);

            int resutCount = await SaveChangesAsync();

            return resutCount == 1;
        }

        public async Task<bool> SoftDeleteTaskAsync(MyTask task)
        {
            task.IsDeleted = true;

            DbContext!.Tasks.Update(task);

            int resutCount = await SaveChangesAsync();

            return resutCount == 1;
        }

        public async Task<bool> HardDeleteTaskAsync(MyTask task)
        {
            DbContext!.Tasks.Remove(task);

            int resutCount = await SaveChangesAsync();

            return resutCount == 1;
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await DbContext!.Tasks
                .AnyAsync(t => t.Id == Guid.Parse(id) && !t.IsDeleted);
        }

        public async Task<int> CountAsync(string userId, Expression<Func<MyTask, bool>>? filterQuery)
        {
            IQueryable<MyTask> tasksFetchQuery = DbContext!.Tasks
                .AsNoTracking()
                .Where(h => h.UserId.ToString() == userId)
                .Where(h => !h.IsDeleted);

            if (filterQuery != null)
            {
                tasksFetchQuery = tasksFetchQuery
                    .Where(filterQuery)
                    .AsQueryable();
            }

            return await tasksFetchQuery.CountAsync();
        }
    }
}
