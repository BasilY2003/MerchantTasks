
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

            MerchantBranches branch = new MerchantBranches();
            branch.Address = branchRequest.Address;
            branch.IsMainBranch = branchRequest.IsMainBranch;
            branch.CityId = branchRequest.CityId;
            branch.BranchName = branchRequest.BranchName;
            branch.ContactPersonId = branchRequest.ContactPersonId;
            branch.GovernateId = branchRequest.GovernateId;
            branch.Region = branchRequest.Region;
            branch.Mobile = branchRequest.Mobile;
            branch.Phone = branchRequest.Phone;
            branch.Status = branchRequest.Status;
            branch.Merchant = merchant;

            branch.AlHai = branchRequest.AlHai != null ? branchRequest.AlHai : null;
            branch.Fax = branchRequest.Fax != null ? branchRequest.Fax : null;
            branch.Website = branchRequest.Website != null ? branchRequest.Website : null;
            branch.Gps = branchRequest.Gps != null ? branchRequest.Gps : null;

            await _session.SaveAsync(merchant);
            await tx.CommitAsync();

            return branch;

        }









    }
}
