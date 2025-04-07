using BlogApp.Models.Entities;
using BlogApp.Models.Repositories;
using BlogApp.Services.Interfaces;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogService(IBlogRepository blogRepository, IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _blogRepository = blogRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<BlogViewModel>> GetAllBlogsAsync()
        {
            var blogs = await _blogRepository.GetAllAsync();
            return blogs.Select(b => new BlogViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                PublishDate = b.PublishDate,
                ImageUrl = b.ImageUrl,
                AuthorUsername = b.User?.Username ?? "Bilinmeyen Yazar",
                CategoryName = b.Category?.Name ?? "Bilinmeyen Kategori"
            }).ToList();
        }

        public async Task<BlogViewModel?> GetBlogByIdAsync(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return null;
            }
            return new BlogViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                PublishDate = blog.PublishDate,
                ImageUrl = blog.ImageUrl,
                AuthorUsername = blog.User?.Username ?? "Bilinmeyen Yazar",
                CategoryName = blog.Category?.Name ?? "Bilinmeyen Kategori"
            };
        }

        public async Task<BlogCreateViewModel> GetBlogCreateViewModelAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return new BlogCreateViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };
        }

        public async Task CreateBlogAsync(BlogCreateViewModel model, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var category = await _categoryRepository.GetByIdAsync(model.CategoryId);

            if (user != null && category != null)
            {
                var newBlog = new Blog
                {
                    Title = model.Title,
                    Content = model.Content,
                    PublishDate = DateTime.Now,
                    ImageUrl = model.ImageUrl,
                    UserId = userId,
                    CategoryId = model.CategoryId
                };
                await _blogRepository.AddAsync(newBlog);
            }
            // Hata yönetimi eklenebilir.
        }

        public async Task UpdateBlogAsync(BlogEditViewModel model)
        {
            var existingBlog = await _blogRepository.GetByIdAsync(model.Id);
            var category = await _categoryRepository.GetByIdAsync(model.CategoryId);

            if (existingBlog != null && category != null)
            {
                existingBlog.Title = model.Title;
                existingBlog.Content = model.Content;
                existingBlog.ImageUrl = model.ImageUrl;
                existingBlog.CategoryId = model.CategoryId;
                await _blogRepository.UpdateAsync(existingBlog);
            }
            // Hata yönetimi eklenebilir.
        }

        public async Task DeleteBlogAsync(int id)
        {
            var blogToDelete = await _blogRepository.GetByIdAsync(id);
            if (blogToDelete != null)
            {
                await _blogRepository.DeleteAsync(blogToDelete);
            }
            // Hata yönetimi eklenebilir.
        }

        public async Task<List<BlogViewModel>> GetBlogsByCategoryAsync(int categoryId)
        {
            var blogs = await _blogRepository.GetAllAsync();
            return blogs.Where(b => b.CategoryId == categoryId)
                .Select(b => new BlogViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    PublishDate = b.PublishDate,
                    ImageUrl = b.ImageUrl,
                    AuthorUsername = b.User?.Username ?? "Bilinmeyen Yazar",
                    CategoryName = b.Category?.Name ?? "Bilinmeyen Kategori"
                }).ToList();
        }

        public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
    }
}