namespace iGrow.Web.ViewModels.MyTask
{
    public class MyTaskDetailsViewModel
    {
        public string Title { get; set; } = null!;
        public string Date { get; set; } = null!;
        public int Priority { get; set; }
        public string? Note { get; set; }
        public bool IsCompleted { get; set; }
        public string RecurringTypeName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;

    }
}
