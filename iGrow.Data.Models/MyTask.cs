namespace iGrow.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static iGrow.GCommon.ValidationConstants;

    [Table("Tasks")]
    public class MyTask
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(TaskTitleMaxLength)]
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        [Required]
        [Range(TaskPriorityMinValue, TaskPriorityMaxValue)]
        public int Priority { get; set; }
        [MaxLength(TaskNoteMaxLength)]
        public string? Note { get; set; }
        public bool IsCompleted { get; set; }
        public int RecurringTypeId { get; set; }
        public virtual RecurringType? RecurringType { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        [Required]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;
    }
}