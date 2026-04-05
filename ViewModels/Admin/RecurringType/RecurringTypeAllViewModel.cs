namespace iGrow.Web.ViewModels.Admin.RecurringType
{
    using System.Collections.Generic;

    public class RecurringTypeAllViewModel
    {
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int ShowingPages { get; set; } = 5;
        public int StartPageIndex { get; set; } = 1;
        public ICollection<RecurringTypeViewModel> RecurringTypes { get; set; } = new List<RecurringTypeViewModel>();
    }
}