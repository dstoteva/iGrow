
namespace iGrow.Web.ViewModels.MyTask
{
    using System.ComponentModel.DataAnnotations;
    using static iGrow.GCommon.ApplicationConstants;
    public class MyTaskDetailsViewModel
    {
        public string Id { get; set; } = null!; 
        public string Title { get; set; } = null!;
        [DataType(DataType.Date)]
        public string Date { get; set; } = null!;
        public int Priority { get; set; }
        public string? Note { get; set; }
        public bool IsCompleted { get; set; }
        public string RecurringTypeName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;

    }
}
