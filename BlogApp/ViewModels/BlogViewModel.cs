using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [StringLength(200, ErrorMessage = "{0} alanı en fazla {1} karakter olabilir.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = null!;
        [Display(Name = "Yayınlanma Tarihi")]
        public DateTime PublishDate { get; set; }
        public string? ImagePath { get; set; }
        [Display(Name = "Yazar Adı")]
        public string AuthorUsername { get; set; } // Yazarın kullanıcı adı
        public int UserId { get; set; } // Blogun sahibinin User Id'si
        [Display(Name = "Kategori")]
        public string CategoryName { get; set; } // Kategorinin adı
        public int CategoryId { get; set; }
        public List<CommentViewModel>? Comments { get; set; }
    }
}