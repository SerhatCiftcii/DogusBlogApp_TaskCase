using BlogApp.ViewModels;

public interface IBlogService
{
    Task<List<BlogViewModel>> GetAllBlogsAsync();
    Task<BlogViewModel?> GetBlogByIdAsync(int id);
    Task<BlogCreateViewModel> GetBlogCreateViewModelAsync(); // Yeni metot
    Task CreateBlogAsync(BlogCreateViewModel model, int userId);
    Task UpdateBlogAsync(BlogEditViewModel model);
    Task DeleteBlogAsync(int id);
    Task<List<BlogViewModel>> GetBlogsByCategoryAsync(int categoryId);
    Task<List<CategoryViewModel>> GetAllCategoriesAsync();
}