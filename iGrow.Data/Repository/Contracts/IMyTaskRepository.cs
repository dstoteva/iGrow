using iGrow.Data.Models;
using System.Linq.Expressions;

namespace iGrow.Data.Repository.Contracts
{
    public interface IMyTaskRepository
    {
        Task<IEnumerable<MyTask>> GetAllTasksNoTrackingByUserIdWithCategoryAndRecurringTypeAsync(string userId, Expression<Func<MyTask, bool>>? filterQuery, int? skipCnt, int? takeCnt);

        Task<IEnumerable<MyTask>> GetAllTasksAsync(string userId);

        Task<MyTask?> GetTaskByIdAsync(string id);

        Task<bool> AddTaskAsync(MyTask task);

        Task<bool> EditTaskAsync(MyTask task);

        Task<bool> SoftDeleteTaskAsync(MyTask task);

        Task<bool> HardDeleteTaskAsync(MyTask task);

        Task<bool> ExistsByIdAsync(string id);

        Task<int> CountAsync(string userId, Expression<Func<MyTask, bool>>? filterQuery);
    }
}
