﻿using CommonLib.Interfaces;
using CommonLib.Services;
using CommonLib.Utils;
using DataLib.Interfaces;
using DataLib.Models;
using Moq;

namespace ProjectUnitTests.Tests
{
    public class AuthenticationTest
    {
           private readonly Mock<IUserRepository> _userRepoMock;
           private readonly Mock<IJwtTokenRepository> _tokenRepoMock;
           private readonly Mock<IJwtService> _jwtServiceMock;
           private readonly Mock<IPasswordHasher> _PasswordHasherMock;
           private readonly IAuthenticationService _authService;

           public AuthenticationTest()
            {
                _userRepoMock = new Mock<IUserRepository>();
                _tokenRepoMock = new Mock<IJwtTokenRepository>();
                _jwtServiceMock = new Mock<IJwtService>();
                _PasswordHasherMock = new Mock<IPasswordHasher>();

                _authService = new AuthenticationService(
                    _userRepoMock.Object,
                    _tokenRepoMock.Object,
                    _jwtServiceMock.Object,
                    _PasswordHasherMock.Object
                );
            }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenNotFound()
        {
            _userRepoMock.Setup(repo => repo.GetByUsernameAsync("Basil"))
                .ReturnsAsync((User?) null);

            var result = await _authService.LoginAsync("Basil", "12345678");
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenFound()
        {
            var plainPassword = "hashedPassword";
            var storedHash = "1234";

            _PasswordHasherMock
                    .Setup(p => p.VerifyPassword(plainPassword, storedHash))
                    .Returns(true);

            _tokenRepoMock.Setup(repo => repo.GetTokenByUserIdAsync(1)).ReturnsAsync((JwtToken?) null);
          
            _userRepoMock.Setup(repo => repo.GetByUsernameAsync("Basil"))
                .ReturnsAsync(new User
                {
                    Id = 1,
                    Username = "Basil",
                    PasswordHash = "1234",
                    Role = "Admin"
                });

            _jwtServiceMock
                .Setup(s => s.GenerateToken(It.IsAny<User>()))
                .Returns("fake-jwt-token");

            var result = await _authService.LoginAsync("Basil", plainPassword);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnTrue_WhenUsernameIsAvailable()
        {
            // Arrange
            var username = "basil";
            var password = "password123";
            var hashedPassword = "hashedPassword123";

            _userRepoMock.Setup(r => r.UsernameExistsAsync(username)).ReturnsAsync(false);
            _PasswordHasherMock.Setup(h => h.HashPassword(password)).Returns(hashedPassword);
            _userRepoMock.Setup(r => r.SaveAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var result = await _authService.RegisterAsync(username, password);

            Assert.True(result);
            _userRepoMock.Verify(r => r.SaveAsync(It.Is<User>(u =>
                u.Username == username && u.PasswordHash == hashedPassword
            )), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFalse_WhenUsernameExists()
        {
            var username = "basil";
            var password = "password123";

            _userRepoMock.Setup(r => r.UsernameExistsAsync(username)).ReturnsAsync(true);

            var result = await _authService.RegisterAsync(username, password);

            Assert.False(result);
            _userRepoMock.Verify(r => r.SaveAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
