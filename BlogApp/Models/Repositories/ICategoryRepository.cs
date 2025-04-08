using BlogApp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Models.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category); 
        Task DeleteAsync(Category category); 
    }
}