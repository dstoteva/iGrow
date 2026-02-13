namespace iGrow.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static iGrow.GCommon.ValidationConstants;

    public class RecurringType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(RecurringTypeNameMaxLength, MinimumLength = RecurringTypeNameMinLength)]
        public string Name { get; set; } = null!;
        public virtual ICollection<MyTask> Tasks { get; set; } = new HashSet<MyTask>();
        public virtual ICollection<Habit> Habits { get; set; } = new HashSet<Habit>();
    }
}
