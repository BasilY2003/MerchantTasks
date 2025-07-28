namespace CommonLib.Interfaces
{
    public interface IAesEncryptionService
    {
        (byte[] EncryptedData, byte[] Key, byte[] IV) Encrypt(string plainText);
        string Decrypt(byte[] cipherText, byte[] key, byte[] iv);
    }
}
