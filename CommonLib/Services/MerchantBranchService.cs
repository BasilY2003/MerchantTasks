using CommonLib.Models;
using CommonLib.RequestBody;
using DataLib.Repository;

namespace CommonLib.Services
{
    public class MerchantBranchService
    {
        private readonly BranchRepository _branchRepository;
        private readonly MerchantRepository _merchantRepository;

        public MerchantBranchService( BranchRepository branchRepository,MerchantRepository merchantRepository)
        {
            _branchRepository = branchRepository;
            _merchantRepository = merchantRepository;
        }
     
        public async Task<List<MerchantBranches>> GetAllBranchesAsync()
        {
            return await _branchRepository.GetAllBranches();
        }

        public async Task<MerchantBranches?> AddBranchAsync(BranchRequest branchBody,long merchantId)
        {

           var merchant = await _merchantRepository.GetMerchantByIdAsync(merchantId).ConfigureAwait(false);
           if (merchant == null) return null;

            MerchantBranches branch = new MerchantBranches {
                Merchant = merchant,
                UpdatedAt = DateTime.UtcNow,
                Address = branchBody.Address,
                Phone = branchBody.Phone,
                IsMainBranch = branchBody.IsMainBranch,
                CityId = branchBody.CityId,
                ContactPersonId = branchBody.ContactPersonId,
                CreatedAt = DateTime.UtcNow,
                Fax = branchBody.Fax,
                BranchName = branchBody.BranchName,
                Gps = branchBody.Gps,
            };
            return branch;
        }
    }
}
