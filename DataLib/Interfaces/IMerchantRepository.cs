using CommonLib.Models;
using CommonLib.RequestBody;

namespace DataLib.Interfaces
{
    public interface IMerchantRepository
    {
        Task<Merchants?> GetMerchantWithGroupByIdAsync(long id);
        Task<Merchants?> GetMerchantByIdAsync(long id);
        Task<List<Merchants>> GetAllMerchantsAsync();
        Task<List<Merchants>> GetMerchantsByGroupIdAsync(long groupId);
        Task<Merchants> UpdateMerchantAsync(Merchants merchant);
        Task<Merchants> AddMerchantAsync(Merchants merchant);
        Task<Merchants> GetMerchantWithMainBranch(long merchantId);
        Task<Merchants?> GetMerchantWithMainBranchTEST(long merchantId);
        Task<List<Merchants>> SearchMerchants(SearchRequest request);
        Task<List<Merchants>> SearchMerchantsWithBranches(SearchRequest request);
    }
}
