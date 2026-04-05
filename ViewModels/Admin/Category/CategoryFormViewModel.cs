namespace iGrow.Web.ViewModels.Admin.Category
{
    using System.ComponentModel.DataAnnotations;
    using static iGrow.GCommon.ValidationConstants;

    public class CategoryFormViewModel
    {
        [Required]
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength)]
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}