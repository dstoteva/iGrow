namespace iGrow.Web.ViewModels.Admin.Amount
{
    using System.ComponentModel.DataAnnotations;
    using static iGrow.GCommon.ValidationConstants;

    public class AmountFormViewModel
    {
        [Required]
        [StringLength(AmountNameMaxLength, MinimumLength = AmountNameMinLength)]
        public string Name { get; set; } = null!;
    }
}