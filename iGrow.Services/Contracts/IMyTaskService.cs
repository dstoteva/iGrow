namespace iGrow.Services.Contracts
{
    using iGrow.ViewModels;
    public interface IMyTaskService
    {
        Task AddTaskAsync(MyTaskCreateViewModel model);
    }
}
