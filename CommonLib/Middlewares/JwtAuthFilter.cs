using CommonLib.Interfaces;
using CommonLib.Localization;
using DataLib.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CommonLib.Middlewares
{
    public class JwtAuthFilter : IAsyncAuthorizationFilter
    {
        private readonly IJwtService _jwtService;
        private readonly IJwtTokenRepository _tokenRepo;

        public JwtAuthFilter(IJwtService jwtService,IJwtTokenRepository tokenRepo)
        {
            _jwtService = jwtService;
            _tokenRepo = tokenRepo;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var errorMessage = LocalizedMessage.GetMessage("UnAuthorized");

            var errorResponse = new ErrorResponse
            {
                Details = null,
                ResponseMessage = errorMessage,
                StatusCode = ErrorCode.UnAuthorized,
            };

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new JsonResult(errorResponse) { StatusCode = 401 };
                return;
            }

            ClaimsPrincipal? principal;
            try
            {
                principal = _jwtService.ValidateToken(token);
            }
            catch (SecurityTokenExpiredException ex)
            {
                var expiredMessage = LocalizedMessage.GetMessage("TokenExpired");

                var expiredResponse = new ErrorResponse
                {
                    StatusCode = ErrorCode.UnAuthorized,
                    ResponseMessage = expiredMessage,
                    Details = ex.Message
                };

                context.Result = new JsonResult(expiredResponse) { StatusCode = 401 };
                return;
            }
            catch (Exception ex)
            {
                var internalMessage = LocalizedMessage.GetMessage("InternalServerError");

                var internalError = new ErrorResponse
                {
                    StatusCode = ErrorCode.InternalServerError,
                    ResponseMessage = internalMessage,
                    Details = ex.Message
                };

                context.Result = new JsonResult(internalError) { StatusCode = 500 };
                return;
            }

            if (principal == null)
            {
                context.Result = new JsonResult(errorResponse) { StatusCode = 401 };
                return;
            }

            var userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!long.TryParse(userIdStr, out var userId))
            {
                context.Result = new JsonResult(errorResponse) { StatusCode = 401 };
                return;
            }

            var tokenFromDb = await _tokenRepo.GetTokenByUserIdAsync(userId);
            if (tokenFromDb == null || tokenFromDb.Token != token || tokenFromDb.IsRevoked)
            {
                context.Result = new JsonResult(errorResponse) { StatusCode = 401 };
                return;
            }
            context.HttpContext.User = principal;
        }
    }
}
