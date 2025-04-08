using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BlogApp.Models.Entities
{
    public class Blog
    {
        [Key]
        [Display(Name = "Blog ID")]
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

        [Display(Name = "Görsel Yolu")] // Değiştirildi
        public string? ImagePath { get; set; } // Değiştirildi

        // Foreign Key for User
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Yazar ID")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [Display(Name = "Yazar")]
        public User User { get; set; } = null!;

        // Foreign Key for Category
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Kategori ID")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [Display(Name = "Kategori")]
        public Category Category { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}