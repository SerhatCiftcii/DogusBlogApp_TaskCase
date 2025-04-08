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
        Task UpdateCategoryAsync(CategoryViewModel model);
        Task DeleteCategoryAsync(int id);
    }
}