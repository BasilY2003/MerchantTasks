using CommonLib.Models;
using CommonLib.RequestBody;

namespace CommonLib.Interfaces
{
    public interface IMerchantBranchService
    {
        Task<List<MerchantBranches>> GetAllBranchesAsync();
        Task<MerchantBranches?> AddBranchAsync(BranchRequest branchBody, long merchantId);
    }
}
