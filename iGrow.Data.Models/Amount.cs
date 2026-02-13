namespace iGrow.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static iGrow.GCommon.ValidationConstants;
    public class Amount
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(AmountNameMaxLength, MinimumLength = AmountNameMinLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Habit> Habits { get; set; } = new HashSet<Habit>();
    }
}
