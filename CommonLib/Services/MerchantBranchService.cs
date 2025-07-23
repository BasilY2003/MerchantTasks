using CommonLib.Interfaces;
using CommonLib.Models;
using CommonLib.RequestBody;
using DataLib.Interfaces;

namespace CommonLib.Services
{
    public class MerchantBranchService : IMerchantBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMerchantRepository _merchantRepository;

        public MerchantBranchService(IBranchRepository branchRepository,IMerchantRepository merchantRepository)
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
