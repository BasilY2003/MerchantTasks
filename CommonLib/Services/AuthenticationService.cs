using CommonLib.Interfaces;
using CommonLib.Utils;
using DataLib.Interfaces;
using DataLib.Models;

namespace CommonLib.Services
{
    public class AuthenticationService  : IAuthenticationService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtTokenRepository _tokenRepo;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly EmailService _emailService;


        public AuthenticationService(
            IUserRepository userRepo,
            IJwtTokenRepository tokenRepo,
            IJwtService jwtService,
            IPasswordHasher passwordHasher,
            EmailService emailService)
        {
            _userRepo = userRepo;
            _tokenRepo = tokenRepo;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            if (await _userRepo.UsernameExistsAsync(username))
                return false;

            var user = new User
            {
                Username = username,
                PasswordHash = _passwordHasher.HashPassword(password)
            };

            await _userRepo.SaveAsync(user);
            return true;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _userRepo.GetByUsernameAsync(username);

            if (user == null || !_passwordHasher.VerifyPassword(password, user.PasswordHash))
                return null;

            var oldToken = await _tokenRepo.GetTokenByUserIdAsync(user.Id);
            if (oldToken != null)
                await _tokenRepo.DeleteTokenByValueAsync(oldToken.Token);

            var token = _jwtService.GenerateToken(user);
            await _tokenRepo.SaveTokenAsync(token,user);
            await _emailService.SendEmailAsync("2003.basil.yousef@gmail.com", "Welcome!", "<h1>This is a test email.</h1>");

            return token;
        }
    }
}
