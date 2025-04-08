using BlogApp.Models.Entities;
using BlogApp.Models.Repositories;
using BlogApp.Services.Interfaces;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogService(IBlogRepository blogRepository, IUserRepository userRepository, ICategoryRepository categoryRepository, ICommentRepository commentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _blogRepository = blogRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _webHostEnvironment = webHostEnvironment;
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
                ImagePath = b.ImagePath,
                AuthorUsername = b.User?.Username ?? "Bilinmeyen Yazar",
                CategoryName = b.Category?.Name ?? "Bilinmeyen Kategori",
                CategoryId = b.CategoryId
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
                ImagePath = blog.ImagePath,
                AuthorUsername = blog.User?.Username ?? "Bilinmeyen Yazar",
                CategoryName = blog.Category?.Name ?? "Bilinmeyen Kategori",
                CategoryId = blog.CategoryId
            };
        }

        public async Task<BlogViewModel?> GetBlogByIdWithCommentsAsync(int id)
        {
            var blog = await _blogRepository.GetBlogByIdWithCommentsAsync(id);
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
                ImagePath = blog.ImagePath,
                AuthorUsername = blog.User?.Username ?? "Bilinmeyen Yazar",
                CategoryName = blog.Category?.Name ?? "Bilinmeyen Kategori",
                CategoryId = blog.CategoryId,
                Comments = blog.Comments.Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    CreatedDate = c.CreatedDate,
                    AuthorUsername = c.User?.Username ?? "Bilinmeyen Yazar"
                }).ToList()
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
            string? imagePath = null;

            if (model.Image != null && model.Image.Length > 0)
            {
                imagePath = await SaveImageAsync(model.Image);
            }

            if (user != null && category != null)
            {
                var newBlog = new Blog
                {
                    Title = model.Title,
                    Content = model.Content,
                    PublishDate = DateTime.Now,
                    ImagePath = imagePath,
                    UserId = userId,
                    CategoryId = model.CategoryId
                };
                await _blogRepository.AddAsync(newBlog);
            }
            // TODO: Hata yönetimi eklenebilir.
        }

        public async Task UpdateBlogAsync(BlogEditViewModel model, string? newImagePath)
        {
            var existingBlog = await _blogRepository.GetByIdAsync(model.Id);
            var category = await _categoryRepository.GetByIdAsync(model.CategoryId);

            if (existingBlog != null && category != null)
            {
                existingBlog.Title = model.Title;
                existingBlog.Content = model.Content;
                existingBlog.CategoryId = model.CategoryId;

                if (!string.IsNullOrEmpty(newImagePath))
                {
                    // Yeni bir görsel yüklendiyse eskiyi sil
                    if (!string.IsNullOrEmpty(existingBlog.ImagePath))
                    {
                        DeleteImage(existingBlog.ImagePath);
                    }
                    existingBlog.ImagePath = newImagePath;
                }

                await _blogRepository.UpdateAsync(existingBlog);
            }
            // TODO: Hata yönetimi eklenebilir.
        }

        public async Task DeleteBlogAsync(int id)
        {
            var blogToDelete = await _blogRepository.GetByIdAsync(id);
            if (blogToDelete != null)
            {
                // Görseli sil
                if (!string.IsNullOrEmpty(blogToDelete.ImagePath))
                {
                    DeleteImage(blogToDelete.ImagePath);
                }
                await _blogRepository.DeleteAsync(blogToDelete);
            }
            // TODO: Hata yönetimi eklenebilir.
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
                    ImagePath = b.ImagePath,
                    AuthorUsername = b.User?.Username ?? "Bilinmeyen Yazar",
                    CategoryName = b.Category?.Name ?? "Bilinmeyen Kategori",
                    CategoryId = b.CategoryId
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

        public async Task<CategoryViewModel?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }
            return new CategoryViewModel { Id = category.Id, Name = category.Name };
        }

        public async Task UpdateCategoryAsync(CategoryViewModel model)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(model.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = model.Name;
                await _categoryRepository.UpdateAsync(existingCategory);
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var categoryToDelete = await _categoryRepository.GetByIdAsync(id);
            if (categoryToDelete != null)
            {
                await _categoryRepository.DeleteAsync(categoryToDelete);
            }
        }

        private async Task<string?> SaveImageAsync(Microsoft.AspNetCore.Http.IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "blog");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Klasörü oluştur (gerekirse)
                Directory.CreateDirectory(uploadsFolder);

                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    return Path.Combine("images", "blog", uniqueFileName); // Veritabanına kaydedilecek yol
                }
                catch (Exception ex)
                {
                    // TODO: Dosya kaydetme hatasını loglayın veya işleyin
                    Console.WriteLine($"Dosya kaydetme hatası: {ex.Message}");
                    return null;
                }
            }
            return null;
        }

        private void DeleteImage(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Dosya silme hatasını loglayın veya işleyin
                    Console.WriteLine($"Dosya silme hatası: {ex.Message}");
                }
            }
        }
    }
}