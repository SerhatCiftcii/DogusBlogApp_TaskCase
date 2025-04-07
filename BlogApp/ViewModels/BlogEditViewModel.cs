using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class BlogEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [StringLength(200, ErrorMessage = "{0} alanı en fazla {1} karakter olabilir.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = null!;

        [Display(Name = "Görsel URL")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        // Kategori seçim listesi için özellik (Açıkça tanımlıyoruz)
        public List<SelectListItem>? Categories { get; set; }
    }
}