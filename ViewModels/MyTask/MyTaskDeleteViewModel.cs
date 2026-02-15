namespace iGrow.Web.ViewModels.MyTask
{
    using System.ComponentModel.DataAnnotations;
    public class MyTaskDeleteViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        [DataType(DataType.Date)]
        public string Date { get; set; } = null!;
    }
}
