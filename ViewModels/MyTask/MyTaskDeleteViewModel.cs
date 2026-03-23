namespace iGrow.Web.ViewModels.MyTask
{
    using System.ComponentModel.DataAnnotations;

    using static iGrow.GCommon.ValidationConstants;
    public class MyTaskDeleteViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required(ErrorMessage = RequiredErrorMessage)]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = RequiredErrorMessage)]
        [DataType(DataType.Date)]
        public string Date { get; set; } = null!;
    }
}
