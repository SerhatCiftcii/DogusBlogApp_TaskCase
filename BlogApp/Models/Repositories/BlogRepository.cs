using BlogApp.Models.Context;
using BlogApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.Models.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Blog>> GetAllAsync()
        {
            return await _context.Blogs
                .Include(b => b.User)
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<Blog?> GetByIdAsync(int id)
        {
            return await _context.Blogs
                .Include(b => b.User)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        
        public async Task<List<Blog>> GetAllAsync(params Expression<Func<Blog, object>>[] includeProperties)
        {
            IQueryable<Blog> query = _context.Blogs;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<Blog?> GetByIdAsync(int id, params Expression<Func<Blog, object>>[] includeProperties)
        {
            IQueryable<Blog> query = _context.Blogs.Where(b => b.Id == id);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task AddAsync(Blog blog)
        {
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Blog blog)
        {
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }

        public async Task<Blog?> GetBlogByIdWithCommentsAsync(int id)
        {
            return await _context.Blogs
                .Where(b => b.Id == id)
                .Include(b => b.User)
                .Include(b => b.Category)
                .Include(b => b.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync();
        }
    }
}