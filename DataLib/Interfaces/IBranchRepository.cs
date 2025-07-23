using CommonLib.Models;
using CommonLib.RequestBody;

namespace DataLib.Interfaces
{
    public interface IBranchRepository
    {
        Task<List<MerchantBranches>> GetAllBranches();
        Task<MerchantBranches?> GetBranchById(long branchId);
        Task<MerchantBranches> AddBranch(BranchRequest branchRequest, Merchants merchant);
    }
}
