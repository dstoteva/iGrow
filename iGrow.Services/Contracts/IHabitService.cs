using iGrow.Web.ViewModels.Habit;

namespace iGrow.Services.Contracts
{
    public interface IHabitService
    {
        Task<IEnumerable<HabitViewModel>> GetAllHabitsAsync(string userId, string? searchQuery, int pageNumber);
        Task AddHabitAsync(HabitFormViewModel model, string userId);
        Task<HabitFormViewModel> GetHabitByIdAsync(string id);
        Task EditHabitAsync(string id, HabitFormViewModel model);
        Task<HabitDetailsViewModel> GetHabitDetailsAsync(string id);
        Task<HabitDeleteViewModel> GetHabitToBeDeletedAsync(string id);
        Task SoftDeleteHabitAsync(string id);
        Task HardDeleteHabitAsync(string id);
        Task<bool> IsUserCreatorAsync(string habitId, string userId);
        Task<int> GetHabitsCountAsync(string userId, string? searchQuery);
    }
}
