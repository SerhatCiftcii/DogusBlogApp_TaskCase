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

        public async Task<List<CommentViewModel>> GetCommentsByBlogIdAsync(int blogId)
        {
            var comments = await _commentRepository.GetCommentsByBlogIdAsync(blogId);
            return comments.Select(c => new CommentViewModel
            {
                Id = c.Id,
                Text = c.Text,
                CreatedDate = c.CreatedDate,
                AuthorUsername = c.User?.Username ?? "Bilinmeyen Yazar",
                BlogId = c.BlogId
            }).ToList();
        }

        public async Task CreateCommentAsync(CommentCreateViewModel model, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                var newComment = new Comment
                {
                    Text = model.Text,
                    CreatedDate = DateTime.Now,
                    UserId = userId,
                    BlogId = model.BlogId
                };
                await _commentRepository.AddAsync(newComment);
            }
            // TODO: Hata yönetimi eklenebilir.
        }
    }
}