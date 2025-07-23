using CommonLib.DTOs;
using CommonLib.RequestBody;

namespace CommonLib.Interfaces
{
    public interface IMerchantService
    {
        Task<MerchantDTO?> GetByIdWithGroupAsync(long id);
        Task<List<MerchantDTO>> GetAllAsync();
        Task<List<MerchantDTO>> GetMerchantsByGroupIdAsync(long groupId);
        Task<MerchantDTO?> AddMerchant(MerchantRequest request, long groupId);
        Task<MerchantDTO?> UpdateMerchant(long merchantId, MerchantRequest request);
        Task<MerchantDTO?> ChangeMerchantGroup(long merchantId, long newGroupId);
        Task<MerchantDTO?> GetMerchantWithMainBranch(long merchantId);
        Task<List<MerchantDTO>> SearchMerchants(SearchRequest request);
    }
}
