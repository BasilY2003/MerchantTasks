using CommonLib.Models;

namespace DataLib.Interfaces
{
    public interface IBranchRepository
    {
        Task<List<MerchantBranches>> GetAllBranches();
        Task<MerchantBranches?> GetBranchById(long branchId);
        Task<MerchantBranches> AddBranch(MerchantBranches branch, Merchants merchant);
    }
}
