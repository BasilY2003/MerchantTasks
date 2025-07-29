namespace CommonLib.Interfaces
{
    public interface IRsaKeyService
    {
        string GenerateAndStoreKeys(long userId);
        byte[] EncryptWithPublicKey(string plainText, string publicKeyPem);
        string DecryptWithPrivateKey(byte[] encryptedData, string privateKeyPem);
    }
}
