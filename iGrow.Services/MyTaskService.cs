namespace iGrow.Services
{
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using iGrow.GCommon.Exceptions;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels.MyTask;

    using static iGrow.GCommon.ApplicationConstants;

    public class MyTaskService : IMyTaskService
    {
        private readonly IMyTaskRepository _taskRepository;

        public MyTaskService(IMyTaskRepository myTaskRepository)
        {
            this._taskRepository = myTaskRepository;
        }

        public async Task<IEnumerable<MyTaskViewModel>> GetAllTasksAsync(string userId, string? searchQuery = null, int pageNumber = 1)
        {
            Expression<Func<MyTask, bool>>? filterQuery = null;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLowerInvariant();

                filterQuery = t => (t.Title.ToLower().Contains(searchQuery));
            }

            int skipCount = (pageNumber - 1) * DefaultEntitiesPerPage;

            IEnumerable<MyTask> tasksFetchQuery = await this._taskRepository
                .GetAllTasksNoTrackingByUserIdWithCategoryAndRecurringTypeAsync(
                    userId: userId,
                    filterQuery: filterQuery,
                    skipCnt: skipCount,
                    takeCnt: DefaultEntitiesPerPage)
                ;

            IEnumerable<MyTaskViewModel> projected = tasksFetchQuery
                .Select(t => new MyTaskViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    Date = t.Date.ToString(MyDateFormat),
                    Priority = t.Priority,
                    Note = t.Note,
                    IsCompleted = t.IsCompleted,
                    RecurringTypeName = t.RecurringType.Name,
                    CategoryName = t.Category.Name,
                    CategoryImageUrl = t.Category.ImageUrl ?? string.Empty
                });

            return projected;
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

            bool success = await this._taskRepository.AddTaskAsync(task);

            if (!success)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<MyTaskFormViewModel> GetTaskByIdAsync(string id)
        {
            MyTask? task = await this._taskRepository
                .GetTaskByIdAsync(id);

            if (task != null)
            {
                return new MyTaskFormViewModel
                    {
                        Title = task.Title,
                        Date = task.Date.ToString(MyDateFormat),
                        Priority = task.Priority,
                        Note = task.Note,
                        IsCompleted = task.IsCompleted,
                        RecurringTypeId = task.RecurringTypeId,
                        CategoryId = task.CategoryId,
                        UserId = task.UserId.ToString()
                };
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task EditTaskAsync(string id, MyTaskFormViewModel model)
        {
            MyTask? task = await this._taskRepository.GetTaskByIdAsync(id);

            if (task != null)
            {
                DateTime taskDate = Convert.ToDateTime(model.Date, CultureInfo.InvariantCulture);

                task.Title = model.Title;
                task.Date = taskDate;
                task.Priority = model.Priority;
                task.Note = model.Note;
                task.IsCompleted = model.IsCompleted;
                task.RecurringTypeId = model.RecurringTypeId;
                task.CategoryId = model.CategoryId;
                task.UserId = Guid.Parse(model.UserId);

                bool success =  await this._taskRepository.EditTaskAsync(task);

                if(!success)
                {
                    throw new EntityPersistFailureException();
                }
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task<MyTaskDetailsViewModel> GetTaskDetailsAsync(string id)
        {
            MyTask? task = await this._taskRepository.GetTaskByIdAsync(id);

            if (task == null)
            {
                throw new EntityNotFoundException();
            }

            MyTaskDetailsViewModel taskToReturn = new MyTaskDetailsViewModel
            {
                Id = task.Id.ToString(),
                Title = task.Title,
                Date = task.Date.ToString(MyDateFormat),
                Priority = task.Priority,
                Note = task.Note,
                IsCompleted = task.IsCompleted,
                RecurringTypeName = task.RecurringType.Name,
                CategoryName = task.Category.Name,
                CategoryImageUrl = task.Category.ImageUrl ?? string.Empty
            };

            return taskToReturn;
        }
        public async Task<MyTaskDeleteViewModel> GetTaskToBeDeletedAsync(string id)
        {
            MyTask? task = await this._taskRepository.GetTaskByIdAsync(id);

            if (task != null && !task.IsDeleted)
            {
                MyTaskDeleteViewModel model = new MyTaskDeleteViewModel
                {
                    Id = task.Id.ToString(),
                    Title = task.Title,
                    Date = task.Date.ToString(MyDateFormat)
                };
                return model;
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task SoftDeleteTaskAsync(string id)
        {
            MyTask? task = await this._taskRepository.GetTaskByIdAsync(id);

            if (task == null)
            {
                throw new EntityNotFoundException();
            }

            bool success = await this._taskRepository.SoftDeleteTaskAsync(task);

            if (!success)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task HardDeleteTaskAsync(string id)
        {
            MyTask? task = await this._taskRepository.GetTaskByIdAsync(id);

            if (task == null)
            {
                throw new EntityNotFoundException();
            }

            bool success = await this._taskRepository.HardDeleteTaskAsync(task);

            if (!success)
            {
                throw new EntityPersistFailureException();
            }
        }
        public async Task<bool> IsUserCreatorAsync(string taskId, string userId)
        {
            MyTask? task = await this._taskRepository.GetTaskByIdAsync(taskId);

            return task != null ? task.UserId.ToString() == userId : false;
        }

        public async Task<int> GetTasksCountAsync(string userId, string? searchQuery = null)
        {
            Expression<Func<MyTask, bool>>? filterQuery = null;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLowerInvariant();

                filterQuery = t => (t.Title.ToLower().Contains(searchQuery));
            }

            int count = await this._taskRepository.CountAsync(userId, filterQuery);

            return count;
        }
    }
}
