using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AuthorUsername { get; set; }
        
    }
}