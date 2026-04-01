namespace iGrow.Web.ViewModels.Habit
{
    public class HabitAllViewModel
    {
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int ShowingPages { get; set; } = 5;
        public int StartPageIndex { get; set; } = 1;
        public ICollection<HabitViewModel> Habits { get; set; } = new List<HabitViewModel>();
    }
}
