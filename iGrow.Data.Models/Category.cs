namespace iGrow.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static iGrow.GCommon.ValidationConstants;
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength)]
        public string Name { get; set; } = null!;
        [MaxLength(CategoryImageUrlMaxLength)]
        public string? ImageUrl { get; set; }
        public virtual ICollection<MyTask> Tasks { get; set; } = new HashSet<MyTask>();
        public virtual ICollection<Habit> Habits { get; set; } = new HashSet<Habit>();
    }
}
