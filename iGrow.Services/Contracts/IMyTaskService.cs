namespace iGrow.Services.Contracts
{
    using iGrow.Web.ViewModels.MyTask;

    public interface IMyTaskService
    {
        Task<IEnumerable<MyTaskAllViewModel>> GetAllTasksAsync(string userId);
        Task AddTaskAsync(MyTaskFormViewModel model, string userId);
        Task<MyTaskFormViewModel> GetTaskByIdAsync(string id);
        Task EditTaskAsync(string id, MyTaskFormViewModel model);
        Task<MyTaskDetailsViewModel> GetTaskDetailsAsync(string id);
        Task<MyTaskDeleteViewModel> GetTaskToBeDeletedAsync(string id);
        Task DeleteTaskAsync(string id);
        Task<bool> IsUserCreatorAsync(string taskId, string userId);
    }
}
