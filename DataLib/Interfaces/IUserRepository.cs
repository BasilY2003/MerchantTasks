using DataLib.Models;

namespace DataLib.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username);
        Task SaveAsync(User user);
    }
}
