using CommonLib.Interfaces;
using CommonLib.Localization;
using CommonLib.RequestBody;
using DataLib.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CommonLib.Middlewares
{
    public class EncryptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, IJwtService _jwtService,IRsaKeyService _rsaKeyService,IAesEncryptionService aesService ,IServerKeyRepository serverKey,IUserService userService)
        {

            if (context.Request.Path.Equals("/api/groups/encryption") || context.Request.Path.Equals("/api/auth/login") || context.Request.Path.Equals("/api/groups/decryption"))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var errorMessage = LocalizedMessage.GetMessage("UnAuthorized");

                var errorResponse = new ResponseMessage
                {
                    Details = null,
                    Message = errorMessage,
                    StatusCode = ResponseCode.UnAuthorized,
                };

                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var json = JsonSerializer.Serialize(errorResponse);
                    await context.Response.WriteAsync(json);
                    return;
                }

                ClaimsPrincipal? principal;
                principal = _jwtService.ValidateToken(token);
               
                if (principal == null) { return; }

                var userIdClaim = principal.FindFirst(ClaimTypes.Name)?.Value;

                   context.Request.EnableBuffering();

                   var serverKeys = await serverKey.GetAsync().ConfigureAwait(false);
                   var privateKeyPem = serverKeys.PrivateKey;

                   // Read the request body as string
                   using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                   var requestBody = await reader.ReadToEndAsync();
                   context.Request.Body.Position = 0;

                   if (IsEncrypted(requestBody))
                   {
                      await DecryptRequestBodyAsync(context, aesService, _rsaKeyService, privateKeyPem);
                   }

                   var originalBody = context.Response.Body;
                   using var newResponseBody = new MemoryStream();
                   context.Response.Body = newResponseBody;

                   await _next(context);

                   if (context.Response.StatusCode > 400)
                   {
                       newResponseBody.Seek(0, SeekOrigin.Begin);
                       await newResponseBody.CopyToAsync(originalBody);
                       return;
                   }

                   await EncryptAndWriteResponseAsync(context, newResponseBody, aesService, _rsaKeyService, userService,
                       userIdClaim,originalBody);
        }

        private async Task DecryptRequestBodyAsync(HttpContext context, IAesEncryptionService aesService, IRsaKeyService rsaKeyService, string privateKeyPem)
        {
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (!IsEncrypted(requestBody))
                return;

            var encryptedRequest = JsonSerializer.Deserialize<AesRequest>(requestBody)
                                   ?? throw new InvalidOperationException("Invalid encrypted request format.");

            var cipherBytes = Convert.FromBase64String(encryptedRequest.EncryptedMessage);
            var keyBytes = Convert.FromBase64String(encryptedRequest.KeyWithIv);

            byte[] decryptedKeyByte = rsaKeyService.DecryptWithPrivateKey(keyBytes, privateKeyPem);
          //  byte[] decryptedIvByte = rsaKeyService.DecryptWithPrivateKey(ivBytes, privateKeyPem);
            var decryptedJson = aesService.Decrypt(cipherBytes, decryptedKeyByte);

            var newBodyBytes = Encoding.UTF8.GetBytes(decryptedJson);
            context.Request.Body = new MemoryStream(newBodyBytes);
            context.Request.ContentLength = newBodyBytes.Length;
            context.Request.ContentType = "application/json";
            context.Request.Body.Seek(0, SeekOrigin.Begin);
        }

        private async Task EncryptAndWriteResponseAsync(
            HttpContext context,
            MemoryStream capturedBody,
            IAesEncryptionService aesService,
            IRsaKeyService rsaKeyService,
            IUserService userService,
            string userIdClaim,
            Stream originalBody)
        {
            // Rewind captured body and read it
            capturedBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(capturedBody).ReadToEndAsync();

            // Encrypt with AES
            var (encryptedBytes, key) = aesService.Encrypt(responseBodyText);

            // Get recipient public key
            var receiver = await userService.GetByUsernameAsync(userIdClaim)
                           ?? throw new InvalidOperationException("Receiver not found.");

            // Encrypt AES key & IV with RSA
            var enKey = rsaKeyService.EncryptWithPublicKey(key, receiver.PublicKey);
         //   var enIv = rsaKeyService.EncryptWithPublicKey(iv, receiver.PublicKey);

            // Wrap into AES request object
            var encryptedResponse = new AesRequest
            {
                EncryptedMessage = Convert.ToBase64String(encryptedBytes),
                KeyWithIv = Convert.ToBase64String(enKey),
            };

            var encryptedJson = JsonSerializer.Serialize(encryptedResponse);

            // Restore original response stream and write
            context.Response.Body = originalBody;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(encryptedJson);
        }


        private bool IsEncrypted(string requestBody)
              {
                  try
                  {
                      var obj = JsonSerializer.Deserialize<AesRequest>(requestBody);
                      return obj?.EncryptedMessage != null && obj.KeyWithIv != null;
                  }
                  catch
                  {
                      return false;
                  }
              }
    }
}