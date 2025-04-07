using System;

namespace BlogApp.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AuthorUsername { get; set; }
        public int BlogId { get; set; } // Yorumun ait olduğu blogun ID'si
    }
}