using CommonLib.Models;
using DataLib.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace DataLib.Repository
{
    public class MerchantGroupRepository : IMerchantGroupRepository
    {
        private readonly ISession _session;

        public MerchantGroupRepository(ISession session)
        {
            _session = session;
        }

        public async Task<MerchantsGroups?> GetGroupByIdAsync(long id)
        {
            var group = await _session.GetAsync<MerchantsGroups>(id);

            if (group != null && group.DeletedAt == null) return group;
            return null;
        }

        public async Task<MerchantsGroups?> GetGroupWithMerchantsByIdAsync(long id)
        {
            var temp = await _session.GetAsync<MerchantsGroups>(id);
     
            return await _session.Query<MerchantsGroups>()
                .Fetch(g => g.Merchants) 
                .Where(g => g.Id == id && g.DeletedAt == null)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);
        }


        public async Task<List<MerchantsGroups>> GetAllAsync()
        {
            return await _session.Query<MerchantsGroups>()
                           .Where(g => g.DeletedAt == null)
                           .ToListAsync()
                           .ConfigureAwait(false);
        }

        public async Task<MerchantsGroups> CreateGroupAsync(MerchantsGroups group)
        {
            using var tx = _session.BeginTransaction();

            await _session.SaveAsync(group).ConfigureAwait(false);
            await tx.CommitAsync().ConfigureAwait(false);

            return group;
        }

        public async Task<MerchantsGroups> UpdateGroupAsync(MerchantsGroups group)
        {
            using var tx = _session.BeginTransaction(); 
            
            await _session.UpdateAsync(group).ConfigureAwait(false);
            await tx.CommitAsync().ConfigureAwait(false);

            return group;
        }

        public async Task<bool> DeleteGroupAsync(MerchantsGroups group)
        {
            using var tx = _session.BeginTransaction();

            group.DeletedAt = DateTime.UtcNow;
            await _session.UpdateAsync(group).ConfigureAwait(false);
            await tx.CommitAsync().ConfigureAwait(false);

            return true;
        }
    }
}
