using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.Entities
{
    public class User
    {
        [Key]
        [Display(Name = "Kullanıcı ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "{0} alanı en fazla {1} karakter olabilir.")]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string PasswordHash { get; set; } = null!;

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = null!;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegistrationDate { get; set; }

        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}