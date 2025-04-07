using BlogApp.Models.Entities;
using BlogApp.Models.Repositories;
using BlogApp.Services.Interfaces;
using BlogApp.ViewModels;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUserAsync(UserRegisterViewModel model)
        {
            if (await _userRepository.GetByUsernameAsync(model.Username) != null)
            {
                return false; // Kullanıcı adı zaten alınmış
            }

            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                RegistrationDate = DateTime.Now
            };

            await _userRepository.AddAsync(newUser);
            return true;
        }

        public async Task<UserLoginViewModel?> LoginUserAsync(UserLoginViewModel model)
        {
            var user = await _userRepository.GetByUsernameAsync(model.Username);

            if (user != null && VerifyPassword(model.Password, user.PasswordHash))
            {
                return new UserLoginViewModel
                {
                    Username = user.Username,
                    RememberMe = model.RememberMe // Gerekirse kullanılacak
                };
            }

            return null; // Giriş başarısız
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes) == hashedPassword;
            }
        }
    }
}