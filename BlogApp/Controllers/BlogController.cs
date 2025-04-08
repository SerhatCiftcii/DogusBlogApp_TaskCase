using BlogApp.Services.Interfaces;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;

        public BlogController(IBlogService blogService, ICommentService commentService)
        {
            _blogService = blogService;
            _commentService = commentService;
        }


        public async Task<IActionResult> Index(int? categoryId)
        {
            var allBlogs = await _blogService.GetAllBlogsAsync();

            if (categoryId.HasValue && categoryId > 0)
            {
                allBlogs = allBlogs.Where(b => b.CategoryId == categoryId).ToList();
            }

            var categories = await _blogService.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;

            return View(allBlogs);
        }
        [Authorize]
        public async Task<IActionResult> MyBlogs()
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myBlogs = await _blogService.GetAllBlogsAsync(); // Tüm blogları al
            var myBlogList = myBlogs.Where(b => b.UserId.ToString() == loggedInUserId).ToList();
            return View(myBlogList); // Yeni bir View'a gönder
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var blogViewModel = await _blogService.GetBlogByIdWithCommentsAsync(id.Value); // BlogViewModel alıyoruz
            if (blogViewModel == null)
            {
                return NotFound();
            }

            return View(blogViewModel); // BlogViewModel view'a gönderiliyor
        }

        [Authorize] // Giriş yapmış kullanıcılar erişebilir
        public async Task<IActionResult> Create()
        {
            var viewModel = await _blogService.GetBlogCreateViewModelAsync();
            return View(viewModel);
        }

        [Authorize] // Giriş yapmış kullanıcılar erişebilir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    await _blogService.CreateBlogAsync(model, userId);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı bilgisi alınamadı.");
                }
            }

            // Validasyon başarısız olursa formu tekrar göster ve kategorileri tekrar yükle
            var viewModel = await _blogService.GetBlogCreateViewModelAsync();
            viewModel.Title = model.Title;
            viewModel.Content = model.Content;
            viewModel.CategoryId = model.CategoryId;
            return View(viewModel);
        }

        [Authorize] // Giriş yapmış kullanıcılar erişebilir
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var blogViewModel = await _blogService.GetBlogByIdAsync(id.Value);
            if (blogViewModel == null)
            {
                return NotFound();
            }

            // Yetkilendirme kontrolü: Sadece kendi yazısını düzenleyebilir
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (blogViewModel.UserId.ToString() != loggedInUserId)
            {
                return Forbid(); // 403 Yetkisiz Erişim hatası döndür
            }

            var categories = await _blogService.GetAllCategoriesAsync();
            var editViewModel = new BlogEditViewModel
            {
                Id = blogViewModel.Id,
                Title = blogViewModel.Title,
                Content = blogViewModel.Content,
                ImagePath = blogViewModel.ImagePath, // Mevcut görselin yolunu gönderiyoruz
                CategoryId = blogViewModel.CategoryId,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == blogViewModel.CategoryId
                }).ToList()
            };

            return View(editViewModel);
        }

        [Authorize] // Giriş yapmış kullanıcılar erişebilir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogEditViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var existingBlog = await _blogService.GetBlogByIdAsync(id);
            if (existingBlog == null)
            {
                return NotFound();
            }

            // Yetkilendirme kontrolü: Sadece kendi yazısını düzenleyebilir
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (existingBlog.UserId.ToString() != loggedInUserId)
            {
                return Forbid(); // 403 Yetkisiz Erişim hatası döndür
            }

            if (ModelState.IsValid)
            {
                string? newImagePath = null;

                if (model.Image != null && model.Image.Length > 0)
                {
                    // Resim kaydetme işi Service katmanında
                    // newImagePath = await _blogService.SaveImageAsync(model.Image);
                }

                await _blogService.UpdateBlogAsync(model, newImagePath);
                return RedirectToAction(nameof(Index));
            }

            // Validasyon başarısız olursa formu tekrar göster ve kategorileri tekrar yükle
            var categories = await _blogService.GetAllCategoriesAsync();
            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == model.CategoryId
            }).ToList();
            return View(model);
        }

        [Authorize] // Giriş yapmış kullanıcılar erişebilir
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var blogViewModel = await _blogService.GetBlogByIdAsync(id.Value);
            if (blogViewModel == null)
            {
                return NotFound();
            }

            // Yetkilendirme kontrolü: Sadece kendi yazısını silebilir
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (blogViewModel.UserId.ToString() != loggedInUserId)
            {
                return Forbid(); // 403 Yetkisiz Erişim hatası döndür
            }

            return View(blogViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            // Yetkilendirme kontrolü: Sadece kendi yazısını silebilir
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (blog.UserId.ToString() != loggedInUserId)
            {
                return Forbid(); // 403 Yetkisiz Erişim hatası döndür
            }

            // Silme işlemini gerçekleştir
            await _blogService.DeleteBlogAsync(id);

            // Blog listesine yönlendir
            return RedirectToAction("Index");
        }

        // Yorum ekleme action'ı
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    await _commentService.CreateCommentAsync(model, userId, model.BlogId);
                    return RedirectToAction("Details", new { id = model.BlogId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı bilgisi alınamadı.");
                }
            }

            // Model geçersizse, blog detay sayfasını hatalarla birlikte tekrar göster
            var blogViewModelWithComments = await _blogService.GetBlogByIdWithCommentsAsync(model.BlogId); // BlogViewModel alıyoruz
            if (blogViewModelWithComments != null)
            {
                return View("Details", blogViewModelWithComments); // BlogViewModel'i view'a gönderiyoruz
            }
            return NotFound();
        }
    }
}