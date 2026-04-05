namespace iGrow.Web.ViewModels.Admin.RecurringType
{
    using System.ComponentModel.DataAnnotations;
    using static iGrow.GCommon.ValidationConstants;

    public class RecurringTypeFormViewModel
    {
        [Required]
        [StringLength(RecurringTypeNameMaxLength, MinimumLength = RecurringTypeNameMinLength)]
        public string Name { get; set; } = null!;
    }
}
