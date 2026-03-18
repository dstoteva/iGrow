using System.ComponentModel.DataAnnotations;

namespace iGrow.Web.ViewModels.Habit
{
    public class HabitDeleteViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        [DataType(DataType.Date)]
        public string StartDate { get; set; } = null!;
        [Required]
        [DataType(DataType.Date)]
        public string EndDate { get; set; } = null!;
    }
}
