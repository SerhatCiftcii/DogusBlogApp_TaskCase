using BlogApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Models.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User - Blog İlişkisi (One to Many)
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Kullanıcı silinirken bloglara dokunma

            // Blog - Category İlişkisi (Many to One)
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Blogs)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Kategoriye bağlı blog varsa silinmeyi engelle

            // Comment - User İlişkisi (Many to One)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Kullanıcı silinirken yorumlara dokunma

            // Comment - Blog İlişkisi (Many to One)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Blog)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.Cascade); // Blog silinince yorumları sil (bu genellikle güvenlidir)

            // Örnek test verileri ekleme (sabit tarihlerle)
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = "test", Email = "admin@example.com", RegistrationDate = new DateTime(2025, 4, 7, 23, 05, 00) }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Teknoloji" },
                new Category { Id = 2, Name = "Seyahat" },
                new Category { Id = 3, Name = "Yemek" }
            );

            modelBuilder.Entity<Blog>().HasData(
                new Blog { Id = 1, Title = "İlk Blog Yazısı", Content = "Bu ilk blog yazısının içeriği...", PublishDate = new DateTime(2025, 4, 2, 23, 05, 00), UserId = 1, CategoryId = 1 },
                new Blog { Id = 2, Title = "Gezilecek Yerler", Content = "Mutlaka görülmesi gereken yerler...", PublishDate = new DateTime(2025, 4, 5, 23, 05, 00), UserId = 1, CategoryId = 2 }
            );

            modelBuilder.Entity<Comment>().HasData(
                new Comment { Id = 1, Text = "Harika bir yazı!", CreatedDate = new DateTime(2025, 4, 4, 23, 05, 00), UserId = 1, BlogId = 1 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}