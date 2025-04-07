using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "{0} alanı en fazla {1} karakter olabilir.")]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}