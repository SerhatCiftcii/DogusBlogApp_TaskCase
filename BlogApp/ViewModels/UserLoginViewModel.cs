using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        [StringLength(50, ErrorMessage = "{0} alanı en fazla {1} karakter olabilir.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = null!;

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}