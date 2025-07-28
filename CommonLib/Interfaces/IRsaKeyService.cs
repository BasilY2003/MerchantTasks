namespace CommonLib.Interfaces
{
    public interface IRsaKeyService
    {
        string GenerateAndStoreKeys(string userId);
    }

}
