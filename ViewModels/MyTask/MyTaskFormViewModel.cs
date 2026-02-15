namespace iGrow.Web.ViewModels.MyTask
{
    using System.ComponentModel.DataAnnotations;

    using static iGrow.GCommon.ValidationConstants;
    public class MyTaskFormViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        [StringLength(TaskTitleMaxLength, MinimumLength = TaskTitleMinLength)]
        public string Title { get; set; } = null!;
        [Required]
        [DataType(DataType.Date)]
        public string Date { get; set; } = null!;
        [Required]
        [Range(TaskPriorityMinValue, TaskPriorityMaxValue)]
        public int Priority { get; set; } = 1;
        [MaxLength(TaskNoteMaxLength)]  
        public string? Note { get; set; }
        public bool IsCompleted { get; set; }
        public int RecurringTypeId { get; set; }
        public IEnumerable<SelectRecurringTypeId> RecurringTypes { get; set; } = new List<SelectRecurringTypeId>();
        public int CategoryId { get; set; }
        public IEnumerable<SelectCategoryId> Categories { get; set; } = new List<SelectCategoryId>();
        public string? UserId { get; set; }
    }
}
