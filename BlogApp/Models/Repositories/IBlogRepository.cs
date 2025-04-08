using BlogApp.Models.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks; 


public interface IBlogRepository
{
    Task<List<Blog>> GetAllAsync(params Expression<Func<Blog, object>>[] includeProperties);
    Task<Blog?> GetByIdAsync(int id, params Expression<Func<Blog, object>>[] includeProperties);
    Task AddAsync(Blog blog);
    Task UpdateAsync(Blog blog);
    Task DeleteAsync(Blog blog);
    Task<Blog?> GetBlogByIdWithCommentsAsync(int id);
}