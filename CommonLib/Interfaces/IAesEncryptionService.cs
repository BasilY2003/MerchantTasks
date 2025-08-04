namespace CommonLib.Interfaces
{
    public interface IAesEncryptionService
    {
        (byte[] EncryptedData, byte[] Key) Encrypt(string plainText);
        string Decrypt(byte[] cipherText, byte[] key);
    }
}
