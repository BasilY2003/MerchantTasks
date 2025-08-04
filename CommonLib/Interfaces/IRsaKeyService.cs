namespace CommonLib.Interfaces
{
    public interface IRsaKeyService
    {
        string GenerateAndStoreKeys(long userId);
        byte[] EncryptWithPublicKey(byte[] plainText, string publicKeyPem);
        byte[] DecryptWithPrivateKey(byte[] encryptedData, string privateKeyPem);
    }
}
