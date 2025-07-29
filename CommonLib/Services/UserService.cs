using CommonLib.Interfaces;
using DataLib.Interfaces;
using DataLib.Models;

namespace CommonLib.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        //public async Task<User?> GetByIdAsync(long id)
        //{
        //    return await _userRepository.ge;
        //}

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userRepository.UsernameExistsAsync(username);
        }

        public async Task SaveAsync(User user)
        {
            await _userRepository.SaveAsync(user);
        }
    }
}