using BlogApp.ViewModels;

public class BlogViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishDate { get; set; }
    public string? ImagePath { get; set; }
    public string AuthorUsername { get; set; } // Yazarın kullanıcı adı
    public string CategoryName { get; set; } // Kategorinin adı
    public int CategoryId { get; set; }
    public List<CommentViewModel>? Comments { get; set; }
}