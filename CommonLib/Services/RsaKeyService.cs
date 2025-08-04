using CommonLib.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CommonLib.Services
{
    public class RsaKeyService : IRsaKeyService
    {
        public string GenerateAndStoreKeys(long userId)
        {
            using var rsa = RSA.Create(2048);

            byte[] privateKey = rsa.ExportRSAPrivateKey();
            byte[] publicKey = rsa.ExportRSAPublicKey();

            string privateKeyPem = ToPem(privateKey, "RSA PRIVATE KEY");
            string publicKeyPem = ToPem(publicKey, "RSA PUBLIC KEY");

            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApp", "keys", userId.ToString());
            Directory.CreateDirectory(appDataPath);

            string privatePath = Path.Combine(appDataPath, "private.pem");
            File.WriteAllText(privatePath, privateKeyPem);

            return publicKeyPem;
        }

        private string ToPem(byte[] keyBytes, string label)
        {
            string base64 = Convert.ToBase64String(keyBytes, Base64FormattingOptions.InsertLineBreaks);
            return $"-----BEGIN {label}-----\n{base64}\n-----END {label}-----";
        }

        public byte[] EncryptWithPublicKey(byte[] data, string publicKeyPem)
        {
            using RSA rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem.ToCharArray());
            return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
        }


        public byte[] DecryptWithPrivateKey(byte[] encryptedData, string privateKeyPem)
        {
            using RSA rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem.ToCharArray());

            byte[] decryptedBytes = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);
            return decryptedBytes;
        }
    }
}