namespace iGrow.Web.ViewModels.MyTask
{
    using System.ComponentModel.DataAnnotations;

    using static iGrow.GCommon.ValidationConstants;
    public class MyTaskFormViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(TaskTitleMaxLength, MinimumLength = TaskTitleMinLength, ErrorMessage = StringLengthErrorMessage)]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = RequiredErrorMessage)]
        [DataType(DataType.Date)]
        public string Date { get; set; } = null!;
        [Required(ErrorMessage = RequiredErrorMessage)]
        [Range(TaskPriorityMinValue, TaskPriorityMaxValue, ErrorMessage = RangeErrorMessage)]
        public int Priority { get; set; } = 1;
        [MaxLength(TaskNoteMaxLength, ErrorMessage = MaxLengthErrorMessage)]  
        public string? Note { get; set; }
        public bool IsCompleted { get; set; }
        public int RecurringTypeId { get; set; }
        public IEnumerable<SelectRecurringTypeId> RecurringTypes { get; set; } = new List<SelectRecurringTypeId>();
        public int CategoryId { get; set; }
        public IEnumerable<SelectCategoryId> Categories { get; set; } = new List<SelectCategoryId>();
        public string? UserId { get; set; }
    }
}
