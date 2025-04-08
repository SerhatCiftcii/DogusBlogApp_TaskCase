using BlogApp.Models.Entities;
using BlogApp.ViewModels;
using System.Threading.Tasks;

namespace BlogApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegisterViewModel model);
        Task<User?> LoginUserAsync(UserLoginViewModel model);
        
    }
}