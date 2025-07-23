namespace CommonLib.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterAsync(string username, string password);
        Task<string?> LoginAsync(string username, string password);
    }
}
