using DataLib.Models;
using System.Security.Claims;

namespace CommonLib.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}