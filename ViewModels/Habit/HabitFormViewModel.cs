namespace iGrow.Web.ViewModels.Habit
{
    using System.ComponentModel.DataAnnotations;

    using static iGrow.GCommon.ValidationConstants;
    public class HabitFormViewModel
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
        [Required(ErrorMessage = RequiredErrorMessage)]
        [Range(TaskPriorityMinValue, TaskPriorityMaxValue, ErrorMessage = RangeErrorMessage)]
        public int Priority { get; set; }
        [MaxLength(HabitNoteMaxLength, ErrorMessage = MaxLengthErrorMessage)]
        public string? Note { get; set; }
        public bool IsCompleted { get; set; }
        public int RecurringTypeId { get; set; }
        public IEnumerable<SelectRecurringTypeId> RecurringTypes { get; set; } = new List<SelectRecurringTypeId>();
        public int AmountId { get; set; }
        public IEnumerable<SelectAmountId> Amounts { get; set; } = new List<SelectAmountId>();
        public int Metric { get; set; }
        public string? Unit { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<SelectCategoryId> Categories { get; set; } = new List<SelectCategoryId>();
    }
}
