using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models.Entities
{
    public class Comment
    {
        [Key]
        [Display(Name = "Yorum ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Yorum Metni")]
        public string Text { get; set; } = null!;

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; }

        // Foreign Key for User
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Kullanıcı ID")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [Display(Name = "Yorum Yapan")]
        public User User { get; set; } = null!;

        // Foreign Key for Blog
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Blog ID")]
        public int BlogId { get; set; }

        [ForeignKey("BlogId")]
        [Display(Name = "Yorum Yapılan Blog")]
        public Blog Blog { get; set; } = null!;
    }
}