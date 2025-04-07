using BlogApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAllCategoriesAsync();
        Task<CategoryViewModel?> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(CategoryViewModel model);
        // Güncelleme ve silme işlemleri de eklenebilir.
    }
}