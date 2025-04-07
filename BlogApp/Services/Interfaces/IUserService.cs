using BlogApp.ViewModels;
using System.Threading.Tasks;

namespace BlogApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserRegisterViewModel model);
        Task<UserLoginViewModel?> LoginUserAsync(UserLoginViewModel model);
        // Diğer kullanıcı işlemleri (örneğin, kullanıcıyı getirme, güncelleme vb.) eklenebilir.
    }
}