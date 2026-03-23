namespace iGrow.Web.ViewModels.Habit
{
    using System.ComponentModel.DataAnnotations;

    using static iGrow.GCommon.ValidationConstants;
    public class HabitDeleteViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required(ErrorMessage = RequiredErrorMessage)]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = RequiredErrorMessage)]
        [DataType(DataType.Date)]
        public string StartDate { get; set; } = null!;
        [Required(ErrorMessage = RequiredErrorMessage)]
        [DataType(DataType.Date)]
        public string EndDate { get; set; } = null!;
    }
}
