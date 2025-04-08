using BlogApp.Models.Entities;
using BlogApp.Models.Repositories;
using BlogApp.Services.Interfaces;
using BlogApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
            return new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task CreateCategoryAsync(CategoryViewModel model)
        {
            var newCategory = new Category
            {
                Name = model.Name
            };
            await _categoryRepository.AddAsync(newCategory);
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
    }
}