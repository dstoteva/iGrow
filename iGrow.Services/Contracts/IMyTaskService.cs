namespace iGrow.Services.Contracts
{
    using iGrow.Web.ViewModels.MyTask;

    public interface IMyTaskService
    {
        Task<IEnumerable<MyTaskViewModel>> GetAllTasksAsync(string userId, string? searchQuery, int pageNumber);
        Task AddTaskAsync(MyTaskFormViewModel model, string userId);
        Task<MyTaskFormViewModel> GetTaskByIdAsync(string id);
        Task EditTaskAsync(string id, MyTaskFormViewModel model);
        Task<MyTaskDetailsViewModel> GetTaskDetailsAsync(string id);
        Task<MyTaskDeleteViewModel> GetTaskToBeDeletedAsync(string id);
        Task SoftDeleteTaskAsync(string id);
        Task HardDeleteTaskAsync(string id);
        Task<bool> IsUserCreatorAsync(string taskId, string userId);
        Task<int> GetTasksCountAsync(string userId, string? searchQuery);
    }
}
