using CommonLib.Models;
using CommonLib.RequestBody;
using NHibernate;
using NHibernate.Linq;

namespace DataLib.Repository
{
    public class MerchantRepository
    {
        private readonly ISession _session;

        public MerchantRepository(ISession session)
        {
            _session = session;
        }

        

        public async Task<Merchants?> GetMerchantWithGroupByIdAsync(long id)
        {
            var merchant = await _session.Query<Merchants>()
                .Where(merchant => merchant.Id == id && merchant.DeletedAt != null)
                .Fetch(merchant => merchant.MerchantsGroup)
                .FirstOrDefaultAsync();

            return merchant;

        }

        public async Task<Merchants?> GetMerchantByIdAsync(long id)
        {
            var merchant = await _session.GetAsync<Merchants>(id);
            if (merchant != null && merchant.DeletedAt == null) return merchant; 
            return null;

        }

        public Task<List<Merchants>> GetAllMerchantsAsync()
        {
            return _session.Query<Merchants>()
                .Where(m => m.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<List<Merchants>> GetMerchantsByGroupIdAsync(long groupId)
        {
            return await _session.Query<Merchants>()
                .Where(m => m.MerchantsGroup.Id == groupId && m.DeletedAt == null)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Merchants> UpdateMerchantAsync(Merchants merchant)
        {
            using var tx = _session.BeginTransaction();

            await _session.UpdateAsync(merchant).ConfigureAwait(false);
            await tx.CommitAsync().ConfigureAwait(false);

            return merchant;
        }

        public async Task<Merchants> AddMerchantAsync(Merchants merchant)
        {
            using var tx = _session.BeginTransaction();

            await _session.SaveAsync(merchant).ConfigureAwait(false);
            await tx.CommitAsync().ConfigureAwait(false);

            return merchant;
        }

        public async Task<Merchants> GetMerchantWithMainBranch(long merchantId)
        {
            return await _session.Query<Merchants>()
                .Where(m => m.Id == merchantId && m.DeletedAt == null &&
                            m.MerchantBranches.Any(b => b.IsMainBranch))
                .Fetch(m => m.MerchantBranches)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);

        }

        public async Task<Merchants> GetMerchantWithMainBranchTEST(long merchantId)
        {
            var result = await _session.Query<Merchants>()
       .Join(
           _session.Query<MerchantBranches>(),
           m => m.Id,
           b => b.Merchant.Id,
           (m, b) => new { m, b }
       )
       .Where(x =>
           x.m.Id == merchantId &&
           x.m.DeletedAt == null &&
           x.b.IsMainBranch &&
           x.b.DeletedAt == null
       )
       .SingleOrDefaultAsync();

            if (result == null) return null;

            result.m.MerchantBranches = new List<MerchantBranches> { result.b };
            return result.m;



        }


        public async Task<List<Merchants>> SearchMerchants(SearchRequest request)
        {
            var query =  _session.Query<Merchants>().Where(m => m.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(m => m.Name.Contains(request.Name));

            if (request.Status == true)
                query = query.Where(m => m.Status);

            if (request.BusinessType.HasValue)
                query = query.Where(m => m.BusinessType == request.BusinessType.Value);

            if (!string.IsNullOrWhiteSpace(request.ManagerName))
                query = query.Where(m => m.ManagerName.Contains(request.ManagerName));

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<Merchants>> SearchMerchantsWithBranches(SearchRequest request)
        {
            var query = _session.Query<Merchants>().Where(m => m.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(m => m.Name.Contains(request.Name));

            if (request.Status == true)
                query = query.Where(m => m.Status);

            if (request.BusinessType.HasValue)
                query = query.Where(m => m.BusinessType == request.BusinessType.Value);

            if (!string.IsNullOrWhiteSpace(request.ManagerName))
                query = query.Where(m => m.ManagerName.Contains(request.ManagerName));

            return await query.Fetch(m => m.MerchantBranches).ToListAsync().ConfigureAwait(false);
        }




    }
}
