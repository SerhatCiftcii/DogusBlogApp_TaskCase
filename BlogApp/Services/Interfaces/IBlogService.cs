using BlogApp.Models.Entities;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Services.Interfaces
{
    public interface IBlogService
    {
        Task<List<BlogViewModel>> GetAllBlogsAsync();
        Task<BlogViewModel?> GetBlogByIdAsync(int id);
        Task<BlogViewModel?> GetBlogByIdWithCommentsAsync(int id); // Yorumlarla birlikte BlogViewModel
        Task<BlogCreateViewModel> GetBlogCreateViewModelAsync();
        Task CreateBlogAsync(BlogCreateViewModel model, int userId);
        Task UpdateBlogAsync(BlogEditViewModel model, string? newImagePath);
        Task DeleteBlogAsync(int id);
        Task<List<BlogViewModel>> GetBlogsByCategoryAsync(int categoryId);
        Task<List<CategoryViewModel>> GetAllCategoriesAsync();
        Task<CategoryViewModel?> GetCategoryByIdAsync(int id);
        Task UpdateCategoryAsync(CategoryViewModel model);
        Task DeleteCategoryAsync(int id);
    }
}