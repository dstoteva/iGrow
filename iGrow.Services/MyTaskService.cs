namespace iGrow.Services
{
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data;
    using iGrow.Data.Models;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels.MyTask;

    using static iGrow.GCommon.ApplicationConstants;
    using iGrow.GCommon.Exceptions;

    public class MyTaskService : IMyTaskService
    {
        private readonly ApplicationDbContext _dbContext;

        public MyTaskService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<MyTaskViewModel>> GetAllTasksAsync(string userId, string? searchQuery = null, int pageNumber = 1)
        {
            int takeCount = DefaultEntitiesPerPage;
            int skipCount = (pageNumber - 1) * takeCount;

            IQueryable<MyTask> tasksFetchQuery = this._dbContext.Tasks
                .AsNoTracking()
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.Title);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLowerInvariant();

                tasksFetchQuery = tasksFetchQuery
                    .Where(t => t.Title.ToLower().Contains(searchQuery))
                    .AsQueryable();
            }

            tasksFetchQuery = tasksFetchQuery
                .Where(t => t.UserId.ToString() == userId)
                .Where(t => !t.IsDeleted)
                .AsQueryable();

            if (skipCount > 0)
            {
                tasksFetchQuery = tasksFetchQuery
                    .Skip(skipCount)
                    .AsQueryable();
            }
            if (takeCount > 0)
            {
                tasksFetchQuery = tasksFetchQuery
                    .Take(takeCount)
                    .AsQueryable();
            }

            return await tasksFetchQuery
                .Include(t => t.RecurringType)
                .Include(t => t.Category)
                .Select(t => new MyTaskViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    Date = t.Date.ToString(MyDateFormat),
                    Priority = t.Priority,
                    Note = t.Note,
                    IsCompleted = t.IsCompleted,
                    RecurringTypeName = t.RecurringType.Name,
                    CategoryName = t.Category.Name
                }).ToListAsync();
        }

        public async Task AddTaskAsync(MyTaskFormViewModel model, string userId)
        {
            DateTime taskDate = Convert.ToDateTime(model.Date, CultureInfo.InvariantCulture);

            MyTask task = new MyTask
            {
                Title = model.Title,
                Date = taskDate,
                Priority = model.Priority,
                Note = model.Note,
                IsCompleted = model.IsCompleted,
                RecurringTypeId = model.RecurringTypeId,
                CategoryId = model.CategoryId,
                UserId = Guid.Parse(userId)
            };
            await this._dbContext.Tasks.AddAsync(task);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<MyTaskFormViewModel> GetTaskByIdAsync(string id)
        {
            MyTaskFormViewModel? task = await this._dbContext.Tasks
                .Where(t => t.Id.ToString() == id)
                .Where(t => !t.IsDeleted)
                .Select(t => new MyTaskFormViewModel
                {
                    Title = t.Title,
                    Date = t.Date.ToString(MyDateFormat),
                    Priority = t.Priority,
                    Note = t.Note,
                    IsCompleted = t.IsCompleted,
                    RecurringTypeId = t.RecurringTypeId,
                    CategoryId = t.CategoryId
                }).FirstOrDefaultAsync();

            if(task != null)
            {
                return task;
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task<bool> EditTaskAsync(string id, MyTaskFormViewModel model)
        {
            MyTask? task = await this._dbContext.Tasks.FirstOrDefaultAsync(t => t.Id.ToString() == id);
            DateTime taskDate = Convert.ToDateTime(model.Date, CultureInfo.InvariantCulture);

            int resultCount = 0;

            if (task != null && !task.IsDeleted)
            {
                task.Title = model.Title;
                task.Date = taskDate;
                task.Priority = model.Priority;
                task.Note = model.Note;
                task.IsCompleted = model.IsCompleted;
                task.RecurringTypeId = model.RecurringTypeId;
                task.CategoryId = model.CategoryId;

                this._dbContext.Update(task);
                resultCount = await this._dbContext.SaveChangesAsync();
            }
            else
            {
                throw new EntityNotFoundException();
            }

            return resultCount == 1;
        }

        public async Task<MyTaskDetailsViewModel> GetTaskDetailsAsync(string id)
        {
            return await this._dbContext.Tasks
                .Where(t => t.Id.ToString() == id)
                .Where(t => !t.IsDeleted)
                .Select(t => new MyTaskDetailsViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    Date = t.Date.ToString(MyDateFormat),
                    Priority = t.Priority,
                    Note = t.Note,
                    IsCompleted = t.IsCompleted,
                    RecurringTypeName = t.RecurringType.Name,
                    CategoryName = t.Category.Name
                }).FirstOrDefaultAsync() ?? await Task.FromResult<MyTaskDetailsViewModel>(null);
        }
        public async Task<MyTaskDeleteViewModel> GetTaskToBeDeletedAsync(string id)
        {
            MyTask? task = await this._dbContext.Tasks.FirstOrDefaultAsync(t => t.Id.ToString() == id);

            if (task != null && !task.IsDeleted)
            {
                MyTaskDeleteViewModel model = new MyTaskDeleteViewModel
                {
                    Id = task.Id.ToString(),
                    Title = task.Title,
                    Date = task.Date.ToString(MyDateFormat)
                };
                return await Task.FromResult(model);
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task SoftDeleteTaskAsync(string id)
        {
            MyTask? task = await this._dbContext.Tasks.FirstOrDefaultAsync(t => t.Id.ToString() == id);

            if (task != null && !task.IsDeleted)
            {
                task.IsDeleted = true;
                this._dbContext.Update(task);

                int resultCount = await this._dbContext.SaveChangesAsync();

                if(resultCount != 1)
                {
                    throw new EntityPersistFailureException();
                }
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task HardDeleteTaskAsync(string id)
        {
            MyTask? task = await this._dbContext.Tasks.FirstOrDefaultAsync(t => t.Id.ToString() == id);

            if (task != null && !task.IsDeleted)
            {
                this._dbContext.Tasks.Remove(task);
                int resultCount = await this._dbContext.SaveChangesAsync();

                if (resultCount != 1)
                {
                    throw new EntityPersistFailureException();
                }
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }
        public async Task<bool> IsUserCreatorAsync(string taskId, string userId)
        {
            MyTask? task = await this._dbContext.Tasks.FirstOrDefaultAsync(t => t.Id.ToString() == taskId);

            if (task != null)
            {
                return task.UserId.ToString() == userId;
            }
            return false;
        }

        public async Task<int> GetTasksCountAsync(string userId, string? searchQuery = null)
        {
            IQueryable<MyTask> tasksFetchQuery = this._dbContext.Tasks
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLowerInvariant();

                tasksFetchQuery = tasksFetchQuery
                    .Where(h => h.Title.ToLower().Contains(searchQuery))
                    .AsQueryable();
            }

            tasksFetchQuery = tasksFetchQuery
                .Where(h => h.UserId.ToString() == userId)
                .Where(h => !h.IsDeleted)
                .AsQueryable();

            int count = await tasksFetchQuery.CountAsync();

            return count;
        }
    }
}
