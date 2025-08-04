using DataLib.Models;

namespace DataLib.Interfaces
{
    public interface IServerKeyRepository
    {
        Task<ServerKey?> GetAsync();
        Task AddAsync(ServerKey keyPair);
    }
}
