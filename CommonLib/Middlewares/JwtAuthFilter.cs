using CommonLib.Localization;
using CommonLib.Services;
using DataLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CommonLib.Middlewares
{
    public class JwtAuthFilter : IAsyncAuthorizationFilter
    {
        private readonly JwtService _jwtService;
        private readonly JwtTokenRepository _tokenRepo;

        public JwtAuthFilter(JwtService jwtService, JwtTokenRepository tokenRepo)
        {
            _jwtService = jwtService;
            _tokenRepo = tokenRepo;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
           
            var errorMessage = LocalizedErrorHelper.Create(ErrorCode.UnAuthorized, "UnAuthorized", Array.Empty<object>());

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new JsonResult(errorMessage);

                return;
            }


            var principal = _jwtService.ValidateToken(token);
            if (principal == null)
            {
                context.Result = new JsonResult(errorMessage);
                return;
            }

            var userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdStr, out var userId))
            {
                context.Result = new JsonResult(errorMessage);
                return;
            }

            var tokenFromDb = await _tokenRepo.GetTokenByUserIdAsync(userId);
            if (tokenFromDb == null || tokenFromDb.Token != token || tokenFromDb.IsRevoked)
            {
                context.Result = new JsonResult(errorMessage);
                return;
            }

            context.HttpContext.User = principal;
        }
    }
}
