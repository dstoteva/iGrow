namespace iGrow.Services
{
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data;
    using iGrow.Services.Contracts;
    using iGrow.Data.Models;
    using iGrow.Web.ViewModels.MyTask;

    using static iGrow.GCommon.ApplicationConstants;

    public class MyTaskService : IMyTaskService
    {
        private readonly ApplicationDbContext _dbContext;

        public MyTaskService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<MyTaskAllViewModel>> GetAllTasksAsync(string userId)
        {
            return await this._dbContext.Tasks
                .Where(t => t.UserId == userId.ToString())
                .Include(t => t.RecurringType)
                .Include(t => t.Category)
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.Title)
                .Select(t => new MyTaskAllViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    Date = t.Date.ToString(DateFormat),
                    Priority = t.Priority,
                    Note = t.Note,
                    IsCompleted = t.IsCompleted,
                    RecurringTypeName = t.RecurringType.Name,
                    CategoryName = t.Category.Name
                }).ToListAsync();
        }

        public async Task AddTaskAsync(MyTaskFormViewModel model)
        {
            MyTask task = new MyTask
            {
                Title = model.Title,
                Date = model.Date,
                Priority = model.Priority,
                Note = model.Note,
                IsCompleted = model.IsCompleted,
                RecurringTypeId = model.RecurringTypeId,
                CategoryId = model.CategoryId
            };
            await this._dbContext.Tasks.AddAsync(task);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<MyTaskFormViewModel> GetTaskById(string id)
        {
            return await this._dbContext.Tasks
                .Where(t => t.Id.ToString() == id)
                .Select(t => new MyTaskFormViewModel
                {
                    Title = t.Title,
                    Date = t.Date,
                    Priority = t.Priority,
                    Note = t.Note,
                    IsCompleted = t.IsCompleted,
                    RecurringTypeId = t.RecurringTypeId,
                    CategoryId = t.CategoryId
                }).FirstOrDefaultAsync() ?? await Task.FromResult<MyTaskFormViewModel>(null);
        }

        public async Task EditTaskAsync(string id, MyTaskFormViewModel model)
        {
            MyTask? task = await this._dbContext.Tasks.FirstOrDefaultAsync(t => t.Id.ToString() == id);

            if (task != null)
            {
                task.Title = model.Title;
                task.Date = model.Date;
                task.Priority = model.Priority;
                task.Note = model.Note;
                task.IsCompleted = model.IsCompleted;
                task.RecurringTypeId = model.RecurringTypeId;
                task.CategoryId = model.CategoryId;

                this._dbContext.Update(task);
                await this._dbContext.SaveChangesAsync();
            }
            else
            {
                await Task.FromResult<MyTaskFormViewModel>(null);
            }
        }

        public async Task<MyTaskDetailsViewModel> GetTaskDetails(string id)
        {
            return await this._dbContext.Tasks
                .Where(t => t.Id.ToString() == id)
                .Select(t => new MyTaskDetailsViewModel
                {
                    Title = t.Title,
                    Date = t.Date.ToString(),
                    Priority = t.Priority,
                    Note = t.Note,
                    IsCompleted = t.IsCompleted,
                    RecurringTypeName = t.RecurringType.Name,
                    CategoryName = t.Category.Name
                }).FirstOrDefaultAsync() ?? await Task.FromResult<MyTaskDetailsViewModel>(null);
        }
    }
}
