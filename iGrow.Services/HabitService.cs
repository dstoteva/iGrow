namespace iGrow.Services
{
    using iGrow.Data;
    using iGrow.Data.Models;
    using iGrow.GCommon.Exceptions;
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
                .Where(h => !h.IsDeleted)
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
            HabitFormViewModel? habit = await this._dbContext.Habits
                .Where(h => h.Id.ToString() == id)
                .Where(h => !h.IsDeleted)
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
                .FirstOrDefaultAsync();

            if(habit != null)
            {
                return habit;
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task<bool> EditHabitAsync(string id, HabitFormViewModel model)
        {
            Habit? habit = await this._dbContext.Habits.FindAsync(Guid.Parse(id));
            DateTime startDate = Convert.ToDateTime(model.StartDate, CultureInfo.InvariantCulture);
            DateTime endDate = Convert.ToDateTime(model.EndDate, CultureInfo.InvariantCulture);

            int resultCount = 0;

            if (habit != null && !habit.IsDeleted)
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
                resultCount = await this._dbContext.SaveChangesAsync();
            }
            else
            {
                throw new EntityNotFoundException();
            }

            return resultCount == 1;
        }

        public async Task<HabitDetailsViewModel> GetHabitDetailsAsync(string id)
        {
            return await this._dbContext.Habits
                .Where(h => h.Id.ToString() == id)
                .Where(h => !h.IsDeleted)
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

            if (habit != null && !habit.IsDeleted)
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
                throw new EntityNotFoundException();
            }
        }

        public async Task SoftDeleteHabitAsync(string id)
        {
            Habit? habit = await this._dbContext.Habits.FirstOrDefaultAsync(h => h.Id.ToString() == id);

            if (habit != null && !habit.IsDeleted)
            {
                habit.IsDeleted = true;
                this._dbContext.Habits.Update(habit);

                int resultCount = await this._dbContext.SaveChangesAsync();

                if (resultCount != 1)
                {
                    throw new EntityPersistFailureException();
                }
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }
        public async Task HardDeleteHabitAsync(string id)
        {
            Habit? habit = await this._dbContext.Habits.FirstOrDefaultAsync(h => h.Id.ToString() == id);

            if (habit != null && !habit.IsDeleted)
            {
                this._dbContext.Habits.Remove(habit);
                int resultCount = await this._dbContext.SaveChangesAsync();

                if(resultCount != 1)
                {
                    throw new EntityPersistFailureException();
                }
            }
            else
            {
                throw new EntityNotFoundException();
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
