namespace iGrow.Services
{
    using iGrow.Data;
    using iGrow.Data.Models;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels.Habit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using static iGrow.GCommon.ApplicationConstants;

    public class HabitService : IHabitService
    {
        private readonly ApplicationDbContext _dbContext;

        public HabitService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<HabitAllViewModel>> GetAllHabitsAsync(string userId)
        {
            return await this._dbContext.Habits
                .Where(h => h.UserId == userId)
                .Include(h => h.RecurringType)
                .Include(h => h.Amount)
                .Include(h => h.Category)
                .OrderBy(h => h.Priority)
                .ThenBy(h => h.Title)
                .Select(h => new HabitAllViewModel
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    StartDate = h.StartDate.ToString(MyDateFormat),
                    EndDate = h.EndDate.ToString(MyDateFormat),
                    Priority = h.Priority,
                    IsCompleted = h.IsCompleted,
                    CategoryName = h.Category.Name
                })
                .ToListAsync(); 
        }

        public async Task AddHabitAsync(HabitFormViewModel model, string userId)
        {
            DateTime startDate = Convert.ToDateTime(model.StartDate, CultureInfo.InvariantCulture);
            DateTime endDate = Convert.ToDateTime(model.EndDate, CultureInfo.InvariantCulture);

            Habit habit = new Habit
            {
                Title = model.Title,
                StartDate = startDate,
                EndDate = endDate,
                Priority = model.Priority,
                Note = model.Note,
                IsCompleted = model.IsCompleted,
                RecurringTypeId = model.RecurringTypeId,
                AmountId = model.AmountId,
                Metric = model.Metric,
                Unit = model.Unit,
                CategoryId = model.CategoryId,
                UserId = userId
            };

            await this._dbContext.Habits.AddAsync(habit);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<HabitFormViewModel> GetHabitByIdAsync(string id)
        {
            return await this._dbContext.Habits
                .Where(h => h.Id.ToString() == id)
                .Select(h => new HabitFormViewModel
                {
                    Title = h.Title,
                    StartDate = h.StartDate.ToString(MyDateFormat),
                    EndDate = h.EndDate.ToString(MyDateFormat),
                    Priority = h.Priority,
                    Note = h.Note,
                    IsCompleted = h.IsCompleted,
                    RecurringTypeId = h.RecurringTypeId,
                    AmountId = h.AmountId,
                    Metric = h.Metric,
                    Unit = h.Unit ?? string.Empty,
                    CategoryId = h.CategoryId
                })
                .FirstOrDefaultAsync() ?? new HabitFormViewModel();
        }

        public async Task EditHabitAsync(string id, HabitFormViewModel model)
        {
            Habit? habit = await this._dbContext.Habits.FindAsync(Guid.Parse(id));
            DateTime startDate = Convert.ToDateTime(model.StartDate, CultureInfo.InvariantCulture);
            DateTime endDate = Convert.ToDateTime(model.EndDate, CultureInfo.InvariantCulture);

            if (habit != null)
            {
                habit.Title = model.Title;
                habit.StartDate = startDate;
                habit.EndDate = endDate;
                habit.Priority = model.Priority;
                habit.Note = model.Note;
                habit.IsCompleted = model.IsCompleted;
                habit.RecurringTypeId = model.RecurringTypeId;
                habit.AmountId = model.AmountId;
                habit.Metric = model.Metric;
                habit.Unit = model.Unit;
                habit.CategoryId = model.CategoryId;

                this._dbContext.Update(habit);
                await this._dbContext.SaveChangesAsync();
            }
            else
            {
                throw new CultureNotFoundException("Habit not found.");
            }
        }

        public async Task<HabitDetailsViewModel> GetHabitDetailsAsync(string id)
        {
            return await this._dbContext.Habits
                .Where(h => h.Id.ToString() == id)
                .Include(h => h.RecurringType)
                .Include(h => h.Amount)
                .Include(h => h.Category)
                .Select(h => new HabitDetailsViewModel
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    StartDate = h.StartDate.ToString(MyDateFormat),
                    EndDate = h.EndDate.ToString(MyDateFormat),
                    Priority = h.Priority,
                    Note = h.Note,
                    IsCompleted = h.IsCompleted,
                    RecurringTypeName = h.RecurringType.Name,
                    AmountName = h.Amount != null ? h.Amount.Name : string.Empty,
                    Metric = h.Metric,
                    Unit = h.Unit ?? string.Empty,
                    CategoryName = h.Category.Name
                })
                .FirstOrDefaultAsync() ?? new HabitDetailsViewModel();
        }

        public async Task<HabitDeleteViewModel> GetHabitToBeDeletedAsync(string id)
        {
            Habit? habit = await this._dbContext.Habits.FirstOrDefaultAsync(h => h.Id.ToString() == id);

            if (habit != null)
            {
                HabitDeleteViewModel model = new HabitDeleteViewModel
                {
                    Id = habit.Id.ToString(),
                    Title = habit.Title,
                    StartDate = habit.StartDate.ToString(MyDateFormat),
                    EndDate= habit.EndDate.ToString(MyDateFormat)
                };
                return await Task.FromResult(model);
            }
            else
            {
                throw new CultureNotFoundException("Habit not found.");
            }
        }

        public async Task DeleteHabitAsync(string id)
        {
            Habit? habit = await this._dbContext.Habits.FirstOrDefaultAsync(h => h.Id.ToString() == id);

            if (habit != null)
            {
                this._dbContext.Habits.Remove(habit);
                await this._dbContext.SaveChangesAsync();
            }
            else
            {
                throw new CultureNotFoundException("Habit not found.");
            }
        }

        public async Task<bool> IsUserCreatorAsync(string habitId, string userId)
        {
            Habit? habit = await this._dbContext.Habits.FirstOrDefaultAsync(t => t.Id.ToString() == habitId);

            if (habit != null)
            {
                return habit.UserId == userId;
            }
            return false;
        }
    }
}
