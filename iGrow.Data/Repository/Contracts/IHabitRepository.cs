namespace iGrow.Data.Repository.Contracts
{
    using System.Linq.Expressions;

    using iGrow.Data.Models;
    public interface IHabitRepository
    {
        Task<IEnumerable<Habit>> GetAllHabitsNoTrackingByUserIdWithCategoryAndRecurringTypeAndAmountAsync(string userId, Expression<Func<Habit, bool>>? filterQuery, int? skipCnt, int? takeCnt);

        Task<IEnumerable<Habit>> GetAllHabitsAsync(string userId);

        Task<Habit?> GetHabitByIdAsync(string id);

        Task<bool> AddHabitAsync(Habit habit);

        Task<bool> EditHabitAsync(Habit habit);

        Task<bool> SoftDeleteHabitAsync(Habit habit);

        Task<bool> HardDeleteHabitAsync(Habit habit);

        Task<bool> ExistsByIdAsync(string id);

        Task<int> CountAsync(string userId, Expression<Func<Habit, bool>>? filterQuery);
    }
}
