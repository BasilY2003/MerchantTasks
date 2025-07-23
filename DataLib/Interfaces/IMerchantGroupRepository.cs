using CommonLib.Models;

namespace DataLib.Interfaces
{
    public interface IMerchantGroupRepository
    {
        Task<MerchantsGroups?> GetGroupByIdAsync(long id);
        Task<MerchantsGroups?> GetGroupWithMerchantsByIdAsync(long id);
        Task<List<MerchantsGroups>> GetAllAsync();
        Task<MerchantsGroups> CreateGroupAsync(MerchantsGroups group);
        Task<MerchantsGroups> UpdateGroupAsync(MerchantsGroups group);
        Task<bool> DeleteGroupAsync(MerchantsGroups group);
    }
}
