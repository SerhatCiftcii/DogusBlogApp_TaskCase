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

        public async Task<User?> LoginUserAsync(UserLoginViewModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);

            if (user != null && VerifyPassword(model.Password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        public string HashPassword(string password)
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