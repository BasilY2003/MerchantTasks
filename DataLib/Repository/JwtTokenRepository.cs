using DataLib.Models;
using NHibernate;
using NHibernate.Linq;

namespace DataLib.Repository
{
    public class JwtTokenRepository
    {
        private readonly ISession _session;

        public JwtTokenRepository(ISession session)
        {
            _session = session;
        }

        public async Task<JwtToken?> GetTokenByUserIdAsync(long userId)
        {
            return await _session.Query<JwtToken>()
                .Where(t => t.UserId == userId && !t.IsRevoked)
                .SingleOrDefaultAsync();
        }

        public async Task DeleteTokenByValueAsync(string tokenString)
        {
            var token = await _session.Query<JwtToken>().FirstOrDefaultAsync(t => t.Token == tokenString);

                await _session.DeleteAsync(token);
                await _session.FlushAsync();
        }

        public async Task SaveTokenAsync(String tokenString,User user)
        {
            var jwtToken = new JwtToken
            {
                User = user,
                Token = tokenString,
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(60),
                IsRevoked = false
            };
            await _session.SaveOrUpdateAsync(jwtToken);
            await _session.FlushAsync();
        }



    }
}