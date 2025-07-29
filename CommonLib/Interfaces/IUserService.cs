using DataLib.Models;

namespace CommonLib.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByUsernameAsync(string username);
        //Task<User?> GetByIdAsync(long id);
        Task<bool> UsernameExistsAsync(string username);
        Task SaveAsync(User user);
    }
}
