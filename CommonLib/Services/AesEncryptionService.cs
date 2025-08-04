using CommonLib.Interfaces;
using System.Security.Cryptography;

namespace CommonLib.Services
{
    public class AesEncryptionService : IAesEncryptionService
    {
        public string Decrypt(byte[] cipherText, byte[] keyWithIv)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (keyWithIv == null || keyWithIv.Length <= 0)
                throw new ArgumentNullException("Key");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyWithIv[..32];
                aesAlg.IV = keyWithIv[32..];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public (byte[] EncryptedData, byte[] Key) Encrypt(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            var keyAndIv= GenerateKeyAndIV();
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyAndIv[..32];
                aesAlg.IV = keyAndIv[32..];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    encrypted = msEncrypt.ToArray();
                }

                return (encrypted,keyAndIv);
            }
        }

        public byte[] GenerateKeyAndIV()
        {
            byte[] keyAndIv = new byte[48];
            RandomNumberGenerator.Fill(keyAndIv);

            return keyAndIv;
        }
    }
}