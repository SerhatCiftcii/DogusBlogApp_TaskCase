using System;

namespace BlogApp.ViewModels
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public string? ImageUrl { get; set; }
        public string AuthorUsername { get; set; }
        public string CategoryName { get; set; }

        // Bu satırı ekleyin:
        public int CategoryId { get; set; }
    }
}