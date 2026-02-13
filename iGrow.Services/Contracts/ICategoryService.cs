using iGrow.Web.ViewModels;

namespace iGrow.Services.Contracts
{
    public interface ICategoryService
    {
        Task <IEnumerable<SelectCategoryId>> GetAllCategoriesAsync();
    }
}
