using iGrow.Web.ViewModels.Habit;

namespace iGrow.Services.Contracts
{
    public interface IHabitService
    {
        Task<IEnumerable<HabitAllViewModel>> GetAllHabitsAsync(string userId);
        Task AddHabitAsync(HabitFormViewModel model, string userId);
        Task<HabitFormViewModel> GetHabitByIdAsync(string id);
        Task<bool> EditHabitAsync(string id, HabitFormViewModel model);
        Task<HabitDetailsViewModel> GetHabitDetailsAsync(string id);
        Task<HabitDeleteViewModel> GetHabitToBeDeletedAsync(string id);
        Task DeleteHabitAsync(string id);
        Task<bool> IsUserCreatorAsync(string habitId, string userId);
    }
}
