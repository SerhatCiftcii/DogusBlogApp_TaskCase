using BlogApp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks; 

namespace BlogApp.Models.Repositories
{
    public interface IBlogRepository
    {
        Task<List<Blog>> GetAllAsync();
        Task<Blog?> GetByIdAsync(int id);
        Task AddAsync(Blog blog);
        Task UpdateAsync(Blog blog);
        Task DeleteAsync(Blog blog);
    }
}