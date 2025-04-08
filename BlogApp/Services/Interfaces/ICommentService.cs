using BlogApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentViewModel?> GetCommentByIdAsync(int id); // EKlendi
        Task<List<CommentViewModel>> GetCommentsByBlogIdAsync(int blogId);
        Task CreateCommentAsync(CommentCreateViewModel model, int userId, int blogId); // BlogId parametresi eklendi
        Task DeleteCommentAsync(int id); // EKlendi
    }
}