using BlogApp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Models.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(int id); // EKlendi
        Task<List<Comment>> GetCommentsByBlogIdAsync(int blogId);
        Task AddAsync(Comment comment);
        Task DeleteAsync(Comment comment); // EKlendi
    }
}