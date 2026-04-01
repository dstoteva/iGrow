namespace iGrow.Web.ViewModels.MyTask
{
    public class MyTaskAllViewModel
    {
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int ShowingPages { get; set; } = 5;
        public int StartPageIndex { get; set; } = 1;
        public ICollection<MyTaskViewModel> Tasks { get; set; } = new List<MyTaskViewModel>();
    }
}
