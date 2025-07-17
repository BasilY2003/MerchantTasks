using DataLib.Models;
using DataLib.Repository;

namespace CommonLib.Services
{
    public class AuthenticationService 
    {
        private readonly UserRepository _userRepo;
        private readonly JwtTokenRepository _tokenRepo;
        private readonly JwtService _jwtService;

        public AuthenticationService(
            UserRepository userRepo,
            JwtTokenRepository tokenRepo,
            JwtService jwtService)
        {
            _userRepo = userRepo;
            _tokenRepo = tokenRepo;
            _jwtService = jwtService;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            if (await _userRepo.UsernameExistsAsync(username))
                return false;

            var user = new User
            {
                Username = username,
                PasswordHash = PasswordHasher.HashPassword(password)
            };

            await _userRepo.SaveAsync(user);
            return true;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _userRepo.GetByUsernameAsync(username);

            if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordHash))
                return null;

            var oldToken = await _tokenRepo.GetTokenByUserIdAsync(user.Id);
            if (oldToken != null)
                await _tokenRepo.DeleteTokenByValueAsync(oldToken.Token);

            var token = _jwtService.GenerateToken(user);
            await _tokenRepo.SaveTokenAsync(token,user);

            return token;
        }
    }

}
