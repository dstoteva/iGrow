namespace iGrow.Web.ViewModels.Admin.Category
{
    using System.Collections.Generic;

    public class CategoryAllViewModel
    {
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int ShowingPages { get; set; } = 5;
        public int StartPageIndex { get; set; } = 1;
        public ICollection<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}