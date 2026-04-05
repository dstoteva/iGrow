namespace iGrow.Web.ViewModels.Admin.Amount
{
    using System.Collections.Generic;

    public class AmountAllViewModel
    {
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int ShowingPages { get; set; } = 5;
        public int StartPageIndex { get; set; } = 1;
        public ICollection<AmountViewModel> Amounts { get; set; } = new List<AmountViewModel>();
    }
}
