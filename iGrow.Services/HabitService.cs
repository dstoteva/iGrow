namespace iGrow.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using iGrow.GCommon.Exceptions;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels.Habit;

    using static iGrow.GCommon.ApplicationConstants;

    public class HabitService : IHabitService
    {
        private readonly IHabitRepository _habitRepository;

        public HabitService(IHabitRepository habitRepository)
        {
            this._habitRepository = habitRepository;
        }

        public async Task<IEnumerable<HabitViewModel>> GetAllHabitsAsync(string userId, string? searchQuery = null, int pageNumber = 1)
        {

            Expression<Func<Habit, bool>>? filterQuery = null;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLowerInvariant();

                filterQuery = h => (h.Title.ToLower().Contains(searchQuery));
            }

            int skipCount = (pageNumber - 1) * DefaultEntitiesPerPage;

            IEnumerable<Habit> habitsFetchQuery = await this._habitRepository
                .GetAllHabitsNoTrackingByUserIdWithCategoryAndRecurringTypeAndAmountAsync(
                    userId: userId,
                    filterQuery: filterQuery,
                    skipCnt: skipCount,
                    takeCnt: DefaultEntitiesPerPage);

            IEnumerable<HabitViewModel> projected = habitsFetchQuery
                .Select(t => new HabitViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    StartDate = t.StartDate.ToString(MyDateFormat),
                    EndDate = t.EndDate.ToString(MyDateFormat),
                    Priority = t.Priority,
                    IsCompleted = t.IsCompleted,
                    CategoryName = t.Category.Name
                });

            return projected;
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
                UserId = Guid.Parse(userId)
            };

            bool success = await this._habitRepository.AddHabitAsync(habit);

            if (!success)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<HabitFormViewModel> GetHabitByIdAsync(string id)
        {
            Habit? habit = await this._habitRepository
                .GetHabitByIdAsync(id);

            if (habit != null)
            {
                return new HabitFormViewModel
                {
                    Title = habit.Title,
                    StartDate = habit.StartDate.ToString(MyDateFormat),
                    EndDate = habit.EndDate.ToString(MyDateFormat),
                    Priority = habit.Priority,
                    Note = habit.Note,
                    IsCompleted = habit.IsCompleted,
                    RecurringTypeId = habit.RecurringTypeId,
                    AmountId = habit.AmountId,
                    Metric = habit.Metric,
                    Unit = habit.Unit ?? string.Empty,
                    CategoryId = habit.CategoryId
                };
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task EditHabitAsync(string id, HabitFormViewModel model)
        {
            Habit? habit = await this._habitRepository.GetHabitByIdAsync(id);

            if (habit != null)
            {
                DateTime startDate = Convert.ToDateTime(model.StartDate, CultureInfo.InvariantCulture);
                DateTime endDate = Convert.ToDateTime(model.EndDate, CultureInfo.InvariantCulture);

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

                bool success = await this._habitRepository.EditHabitAsync(habit);

                if (!success)
                {
                    throw new EntityPersistFailureException();
                }
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task<HabitDetailsViewModel> GetHabitDetailsAsync(string id)
        {
            Habit? habit = await this._habitRepository.GetHabitByIdAsync(id);

            if (habit == null)
            {
                throw new EntityNotFoundException();
            }

            HabitDetailsViewModel habitToReturn = new HabitDetailsViewModel
            {
                Id = habit.Id.ToString(),
                Title = habit.Title,
                StartDate = habit.StartDate.ToString(MyDateFormat),
                EndDate = habit.EndDate.ToString(MyDateFormat),
                Priority = habit.Priority,
                Note = habit.Note,
                IsCompleted = habit.IsCompleted,
                RecurringTypeName = habit.RecurringType.Name,
                AmountName = habit.Amount != null ? habit.Amount.Name : string.Empty,
                Metric = habit.Metric,
                Unit = habit.Unit ?? string.Empty,
                CategoryName = habit.Category.Name
            };

            return habitToReturn;
        }

        public async Task<HabitDeleteViewModel> GetHabitToBeDeletedAsync(string id)
        {
            Habit? habit = await this._habitRepository.GetHabitByIdAsync(id);

            if (habit != null && !habit.IsDeleted)
            {
                HabitDeleteViewModel model = new HabitDeleteViewModel
                {
                    Id = habit.Id.ToString(),
                    Title = habit.Title,
                    StartDate = habit.StartDate.ToString(MyDateFormat),
                    EndDate = habit.EndDate.ToString(MyDateFormat)
                };

                return model;
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task SoftDeleteHabitAsync(string id)
        {
            Habit? habit = await this._habitRepository.GetHabitByIdAsync(id);

            if (habit == null)
            {
                throw new EntityNotFoundException();
            }

            bool success = await this._habitRepository.SoftDeleteHabitAsync(habit);

            if (!success)
            {
                throw new EntityPersistFailureException();
            } 
        }
        public async Task HardDeleteHabitAsync(string id)
        {
            Habit? habit = await this._habitRepository.GetHabitByIdAsync(id);

            if (habit == null)
            {
                throw new EntityNotFoundException();
            }

            bool success = await this._habitRepository.HardDeleteHabitAsync(habit);

            if (!success)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<bool> IsUserCreatorAsync(string habitId, string userId)
        {
            Habit? habit = await this._habitRepository.GetHabitByIdAsync(habitId);

            return habit != null ? habit.UserId.ToString() == userId : false;
        }

        public async Task<int> GetHabitsCountAsync(string userId, string? searchQuery = null)
        {
            Expression<Func<Habit, bool>>? filterQuery = null;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLowerInvariant();

                filterQuery = h => (h.Title.ToLower().Contains(searchQuery));
            }

            int count = await this._habitRepository.CountAsync(userId, filterQuery);

            return count;
        }
    }
}
