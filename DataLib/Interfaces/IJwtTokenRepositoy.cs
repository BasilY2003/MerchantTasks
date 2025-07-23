using DataLib.Models;

namespace DataLib.Interfaces
{
    public interface IJwtTokenRepository
    {
        Task<JwtToken?> GetTokenByUserIdAsync(long userId);
        Task DeleteTokenByValueAsync(string tokenString);
        Task SaveTokenAsync(string tokenString, User user);
    }
}