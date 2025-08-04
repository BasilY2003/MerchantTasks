using DataLib.Interfaces;
using System.Security.Cryptography;
using DataLib.Models;

namespace ApiLib
{
    public static class ServerKeyIntializer
    {
        public static async Task Initialize(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IServerKeyRepository>();
           
            if (await repo.GetAsync() == null)
            {
                using var rsa = RSA.Create(2048);

                byte[] privateKey = rsa.ExportRSAPrivateKey();
                byte[] publicKey = rsa.ExportRSAPublicKey();

                string privateKeyPem = ToPem(privateKey, "RSA PRIVATE KEY");
                string publicKeyPem = ToPem(publicKey, "RSA PUBLIC KEY");

                ServerKey key = new ServerKey
                {
                    PrivateKey = privateKeyPem,
                    PublicKey = publicKeyPem,
                    CreatedAt = DateTime.UtcNow
                };

               await repo.AddAsync(key).ConfigureAwait(false);
            }
        }
        private static string ToPem(byte[] keyBytes, string label)
        {
            string base64 = Convert.ToBase64String(keyBytes, Base64FormattingOptions.InsertLineBreaks);
            return $"-----BEGIN {label}-----\n{base64}\n-----END {label}-----";
        }
     }
}
