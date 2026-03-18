namespace iGrow.Web.ViewModels.Habit
{
    using System.ComponentModel.DataAnnotations;
    public class HabitDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        [DataType(DataType.Date)]
        public string StartDate { get; set; } = null!;
        [DataType(DataType.Date)]
        public string EndDate { get; set; } = null!;
        public int Priority { get; set; }
        public string? Note { get; set; }
        public bool IsCompleted { get; set; }
        public string RecurringTypeName { get; set; } = null!;
        public string AmountName { get; set; } = null!;
        public int Metric { get; set; }
        public string Unit { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
}
