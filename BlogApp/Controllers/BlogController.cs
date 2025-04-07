using BlogApp.Services.Interfaces;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #region Blog Listeleme

        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetAllBlogsAsync();
            return View(blogs);
        }

        #endregion

        #region Blog Detayları

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var blog = await _blogService.GetBlogByIdAsync(id.Value);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        #endregion

        #region Yeni Blog Oluşturma

        public async Task<IActionResult> Create()
        {
            var viewModel = await _blogService.GetBlogCreateViewModelAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                int currentUserId = 1; // Gerçek uygulamada oturumdan alınmalı
                await _blogService.CreateBlogAsync(model, currentUserId);
                return RedirectToAction(nameof(Index));
            }

            // Validasyon başarısız olursa formu tekrar göster ve kategorileri tekrar yükle
            var viewModel = await _blogService.GetBlogCreateViewModelAsync();
            viewModel.CategoryId = model.CategoryId; // Seçili kategoriyi koru
            return View(viewModel);
        }

        #endregion

        #region Blog Düzenleme

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var blog = await _blogService.GetBlogByIdAsync(id.Value);
            if (blog == null)
            {
                return NotFound();
            }

            var categories = await _blogService.GetAllCategoriesAsync();
            var editViewModel = new BlogEditViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                ImageUrl = blog.ImageUrl,
                CategoryId = blog.CategoryId,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == blog.CategoryId // Mevcut kategoriyi seçili yap
                }).ToList()
            };

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogEditViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _blogService.UpdateBlogAsync(model);
                return RedirectToAction(nameof(Index));
            }

            // Validasyon başarısız olursa formu tekrar göster ve kategorileri tekrar yükle
            var categories = await _blogService.GetAllCategoriesAsync();
            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == model.CategoryId // Seçili kategoriyi koru
            }).ToList();
            return View(model);
        }

        #endregion

        #region Blog Silme

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var blog = await _blogService.GetBlogByIdAsync(id.Value);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _blogService.DeleteBlogAsync(id);
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}