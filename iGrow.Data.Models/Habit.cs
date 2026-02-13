namespace iGrow.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using static iGrow.GCommon.ValidationConstants;
    public class Habit
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(HabitTitleMaxLength, MinimumLength = HabitTitleMinLength)]
        public string Title { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        [Range(HabitPriorityMinValue, HabitPriorityMaxValue)]
        public int Priority { get; set; }
        [MaxLength(HabitNoteMaxLength)]
        public string? Note { get; set; }
        public bool IsCompleted { get; set; }
        public int RecurringTypeId { get; set; }
        public virtual RecurringType? RecurringType { get; set; }
        public int AmountId { get; set; }
        public virtual Amount? Amount { get; set; }
        public int Metric { get; set; }
        public string? Unit { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        [Required]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;
    }
}
