using DataLib.Interfaces;
using DataLib.Models;
using NHibernate;
using NHibernate.Linq;

namespace DataLib.Repository
{
    public class ServerKeyRepository : IServerKeyRepository
    {
        private readonly ISession _session;

        public ServerKeyRepository(ISession session)
        {
            _session = session;
        }

        public async Task<ServerKey> GetAsync()
        {

            return await _session.Query<ServerKey>().FirstAsync();
        }

        public async Task AddAsync(ServerKey keyPair)
        {
            using var transaction = _session.BeginTransaction();
            await _session.SaveAsync(keyPair);
            await transaction.CommitAsync();
        }
    }
}