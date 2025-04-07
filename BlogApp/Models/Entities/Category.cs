using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models.Entities
{
    public class Category
    {
        [Key]
        [Display(Name = "Kategori ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "{0} alanı en fazla {1} karakter olabilir.")]
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; } = null!;

        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}