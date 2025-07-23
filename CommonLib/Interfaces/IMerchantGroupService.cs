using CommonLib.DTOs;
using CommonLib.RequestBody;

namespace CommonLib.Interfaces
{
    public interface IMerchantGroupService
    {
        Task<MerchantGroupDto?> GetGroupWithMerchantsById(long id);
        Task<List<MerchantGroupDto>> GetAllGroupsAsync();
        Task<MerchantGroupDto> CreateGroupAsync(MerchantGroupRequest request);
        Task<MerchantGroupDto?> UpdateGroupAsync(MerchantGroupRequest request, long groupId);
        Task<bool> DeleteGroupAsync(long groupId);
    }
}
