using BlogApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentViewModel>> GetCommentsByBlogIdAsync(int blogId);
        Task CreateCommentAsync(CommentCreateViewModel model, int userId);
    }
}