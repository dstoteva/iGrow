namespace iGrow.Services.Contracts
{
    using iGrow.Web.ViewModels.MyTask;

    public interface IMyTaskService
    {
        Task<IEnumerable<MyTaskAllViewModel>> GetAllTasksAsync(string userId);
        Task AddTaskAsync(MyTaskFormViewModel model);
        Task<MyTaskFormViewModel> GetTaskById(string id);
        Task EditTaskAsync(string id, MyTaskFormViewModel model);
        Task<MyTaskDetailsViewModel> GetTaskDetails(string id);
    }
}
