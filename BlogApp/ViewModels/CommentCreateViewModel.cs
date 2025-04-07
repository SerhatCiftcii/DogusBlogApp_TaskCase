using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class CommentCreateViewModel
    {
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [Display(Name = "Yorum")]
        [StringLength(500, ErrorMessage = "{0} alanı en fazla {1} karakter olabilir.")]
        public string Text { get; set; } = null!;

        // Blog ID'si genellikle formda gizli bir alan olarak gönderilir veya
        // yönlendirme bilgisinden alınır. Burada doğrudan modelde olmayabilir.
        public int BlogId { get; set; }
    }
}