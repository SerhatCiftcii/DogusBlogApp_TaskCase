using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class CommentCreateViewModel
    {
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Yorum")]
        [StringLength(500, ErrorMessage = "{0} alanı en fazla {1} karakter olabilir.")]
        public string Text { get; set; } = null!;

        [Required]
        public int BlogId { get; set; } // Eklendi ve zorunlu hale getirildi
    }
}
