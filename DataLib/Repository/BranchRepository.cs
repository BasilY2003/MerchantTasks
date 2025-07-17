
using CommonLib.Models;
using CommonLib.RequestBody;
using NHibernate;
using NHibernate.Linq;

namespace DataLib.Repository
{
    public class BranchRepository
    {

        private readonly ISession _session;


        public BranchRepository(ISession session)
        {
            _session = session;
        }

        public async Task<List<MerchantBranches>> GetAllBranches()
        {
            return await _session.Query<MerchantBranches>()
                .Where(branch => branch.DeletedAt != null)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<MerchantBranches?> GetBranchById(long BranchId)
        {
            var branch = await _session.GetAsync<MerchantBranches>(BranchId);
            if (branch != null && branch.DeletedAt == null) return branch;
            return null;


        }

        public async  Task<MerchantBranches> AddBranch(BranchRequest branchRequest, Merchants merchant)
        {

            var tx = _session.BeginTransaction();

            MerchantBranches branch = new MerchantBranches
            {
                Merchant = merchant,
                Address = branchRequest.Address,
                IsMainBranch = branchRequest.IsMainBranch,
                CityId = branchRequest.CityId,
                BranchName = branchRequest.BranchName,
                ContactPersonId = branchRequest.ContactPersonId,
                GovernateId = branchRequest.GovernateId,
                Region = branchRequest.Region,
                Mobile = branchRequest.Mobile,
                Phone = branchRequest.Phone,
                Status = branchRequest.Status,
                AlHai = branchRequest.AlHai,
                Fax = branchRequest.Fax,
                Website = branchRequest.Website,
                Gps = branchRequest.Gps,
            };



         
           

            await _session.SaveAsync(merchant);
            await tx.CommitAsync();

            return branch;

        }









    }
}
