using DataLib.Models;
using NHibernate;
using NHibernate.Linq;

namespace DataLib.Repository
{
    public class UserRepository
    {
        private readonly ISession _session;

        public UserRepository(ISession session)
        {
            _session = session;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _session.Query<User>()
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _session.Query<User>()
                .AnyAsync(u => u.Username == username);
        }

        public async Task SaveAsync(User user)
        {
            await _session.SaveAsync(user);
            await _session.FlushAsync();
        }
    }
}
