namespace iGrow.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static iGrow.GCommon.ValidationConstants;

    public class RecurringType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(RecurringTypeNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
