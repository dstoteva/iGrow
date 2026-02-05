namespace iGrow.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static iGrow.GCommon.ValidationConstants;
    public class Amount
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(AmountNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
