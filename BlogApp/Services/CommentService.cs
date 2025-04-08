using BlogApp.Models.Entities;
using BlogApp.Models.Repositories;
using BlogApp.Services.Interfaces;
using BlogApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public CommentService(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<CommentViewModel?> GetCommentByIdAsync(int id) // Implemente edildi
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return null;
            }
            return new CommentViewModel
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedDate = comment.CreatedDate,
                AuthorUsername = comment.User?.Username ?? "Bilinmeyen Yazar",
                
            };
        }

        public async Task<List<CommentViewModel>> GetCommentsByBlogIdAsync(int blogId)
        {
            var comments = await _commentRepository.GetCommentsByBlogIdAsync(blogId);
            return comments.Select(c => new CommentViewModel
            {
                Id = c.Id,
                Text = c.Text,
                CreatedDate = c.CreatedDate,
                AuthorUsername = c.User?.Username ?? "Bilinmeyen Yazar",
               
            }).ToList();
        }

        public async Task CreateCommentAsync(CommentCreateViewModel model, int userId, int blogId) // BlogId parametresi güncellendi
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                var newComment = new Comment
                {
                    Text = model.Text,
                    CreatedDate = DateTime.Now,
                    UserId = userId,
                    BlogId = blogId
                };
                await _commentRepository.AddAsync(newComment);
            }
            // TODO: Hata yönetimi eklenebilir.
        }

        public async Task DeleteCommentAsync(int id) // Implemente edildi
        {
            var commentToDelete = await _commentRepository.GetByIdAsync(id);
            if (commentToDelete != null)
            {
                await _commentRepository.DeleteAsync(commentToDelete);
            }
            // TODO: Yetkilendirme kontrolü eklenebilir (sadece yorum sahibi veya admin silebilir).
        }
    }
}