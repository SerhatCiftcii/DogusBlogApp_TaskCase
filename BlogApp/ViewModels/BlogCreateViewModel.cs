using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BlogApp.ViewModels
{
    public class BlogCreateViewModel
    {
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [StringLength(200, ErrorMessage = "{0} alanı en fazla {1} karakter olabilir.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = null!;

        [Display(Name = "Görsel")]
        public IFormFile? Image { get; set; } // Değiştirildi

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        public List<SelectListItem>? Categories { get; set; }
    }
}