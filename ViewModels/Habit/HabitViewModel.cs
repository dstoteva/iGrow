namespace iGrow.Web.ViewModels.Habit
{
    using System.ComponentModel.DataAnnotations;
    public class HabitViewModel
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        [DataType(DataType.Date)]
        public string StartDate { get; set; } = null!;
        [DataType(DataType.Date)]
        public string EndDate { get; set; } = null!;
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryImageUrl { get; set; } = null!;

    }
}
