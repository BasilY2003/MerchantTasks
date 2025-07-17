
using CommonLib.Models;
using DataLib.Repository;

namespace CommonLib.Services
{
    public class MerchantBranchService
    {
        private readonly BranchRepository _branchRepository;

        public MerchantBranchService( BranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        
        public async Task<List<MerchantBranches>> GetAllBranchesAsync()
        {
            return await _branchRepository.GetAllBranches();
        }

        // Get branch by Id
        //public async Task<MerchantBranches?> GetBranchByIdAsync(long branchId)
        //{
        //    return await _branchRepository.GetBranchById(branchId);
        //}

        //// Add a branch (pass the full entity, not request DTO)
        //public async Task<MerchantBranches> AddBranchAsync(MerchantBranches branch)
        //{
        //    using var tx = _session.BeginTransaction();

        //    await _session.SaveAsync(branch);

        //    await tx.CommitAsync();

        //    return branch;
        //}

        //// Update a branch (pass the full entity)
        //public async Task<MerchantBranches> UpdateBranchAsync(MerchantBranches branch)
        //{
        //    using var tx = _session.BeginTransaction();

        //    await _session.UpdateAsync(branch);

        //    await tx.CommitAsync();

        //    return branch;
        //}
    }
}
