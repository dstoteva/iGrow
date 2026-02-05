namespace iGrow.Services
{
    using iGrow.Data;
    using iGrow.Services.Contracts;
    using iGrow.ViewModels;
    public class MyTaskService : IMyTaskService
    {
        private readonly ApplicationDbContext _dbContext;

        public MyTaskService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public Task AddTaskAsync(MyTaskCreateViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
