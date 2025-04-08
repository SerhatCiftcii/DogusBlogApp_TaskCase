using BlogApp.Models.Entities;
using System.Threading.Tasks;

namespace BlogApp.Models.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email); // Yeni metot
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        // Task DeleteAsync(User user); // Henüz implemente edilmedi
    }
}