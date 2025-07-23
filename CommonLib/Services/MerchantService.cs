using CommonLib.DTOs;
using CommonLib.Interfaces;
using CommonLib.Models;
using CommonLib.RequestBody;
using DataLib.Interfaces;
using DataLib.Repository;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CommonLib.Services
{
    public class MerchantService : IMerchantService
    {
        private const string AllMerchantsKey = "merchants:all";
        private const string MerchantByIdPrefix = "merchants:id:";
        private const string ByGroupKeyPrefix = "merchants:group:";

        private readonly IMerchantRepository _merchantRepo;
        private readonly IMerchantGroupRepository _groupRepo;
      
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };

        public MerchantService(IMerchantRepository merchantRepo,
                               IDistributedCache cache,
                               IMerchantGroupRepository groupRepo)
{
            _merchantRepo = merchantRepo;
            _cache = cache;
            _groupRepo = groupRepo;
            _cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
        }

        public async Task<MerchantDTO?> GetByIdWithGroupAsync(long id)
        {
            var key = MerchantByIdPrefix + id;
            Merchants? merchant;

            var cached = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cached))
            {
                merchant = JsonSerializer.Deserialize<Merchants>(cached, _jsonOptions);
            }
            else
            {
                merchant = await _merchantRepo.GetMerchantWithGroupByIdAsync(id);
                if (merchant != null)
                {
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(merchant, _jsonOptions), _cacheOptions);
                }
            }
                return new MerchantDTO
                {
                    Id = merchant.Id,
                    Name = merchant.Name,
                    ManagerName = merchant.ManagerName,
                    BusinessType = merchant.BusinessType,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Status = merchant.Status,
                    MerchantsGroup = new MerchantGroupDto
                    {
                        Id = merchant.MerchantsGroup.Id,
                        Name = merchant.MerchantsGroup.Name,
                        CreatedAt = merchant.MerchantsGroup.CreatedAt,
                        UpdatedAt = merchant.MerchantsGroup.UpdatedAt,
                    }
                };   
        }

        public async Task<List<MerchantDTO>> GetAllAsync()
        {
            List<Merchants>? merchants;
            var cached = await _cache.GetStringAsync(AllMerchantsKey);

            if (!string.IsNullOrEmpty(cached))
            {
                merchants = JsonSerializer.Deserialize<List<Merchants>>(cached, _jsonOptions);
            }
            else
            {
                merchants = await _merchantRepo.GetAllMerchantsAsync();
                await _cache.SetStringAsync(AllMerchantsKey, JsonSerializer.Serialize(merchants, _jsonOptions), _cacheOptions);
            }

            return merchants.Select(merchant => new MerchantDTO
            {  
                Id = merchant.Id,
                Name = merchant.Name,
                ManagerName = merchant.ManagerName,
                BusinessType = merchant.BusinessType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = merchant.Status,
            }).ToList();
        }

        public async Task<List<MerchantDTO>> GetMerchantsByGroupIdAsync(long groupId)
        {
            var key = ByGroupKeyPrefix + groupId;
            List<Merchants>? merchants;

            var cached = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cached))
            {
                merchants = JsonSerializer.Deserialize<List<Merchants>>(cached, _jsonOptions);
            }
            else
            {
                merchants = await _merchantRepo.GetMerchantsByGroupIdAsync(groupId);
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(merchants, _jsonOptions), _cacheOptions);
            }

            return merchants.Select(merchant => new MerchantDTO
            {
                Id = merchant.Id,
                Name = merchant.Name,
                ManagerName = merchant.ManagerName,
                BusinessType = merchant.BusinessType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = merchant.Status,
                MerchantsGroup = new MerchantGroupDto
                {
                    Id = merchant.MerchantsGroup.Id,
                    Name = merchant.MerchantsGroup.Name,
                    CreatedAt = merchant.MerchantsGroup.CreatedAt,
                    UpdatedAt = merchant.MerchantsGroup.UpdatedAt,
                }
            }).ToList();
        }

        public async Task<MerchantDTO?> AddMerchant(MerchantRequest request, long groupId)
        {
            var group = await _groupRepo.GetGroupByIdAsync(groupId);
            if (group == null) return null;

            var merchant = new Merchants
            {
                Name = request.Name,
                ManagerName = request.ManagerName,
                BusinessType = request.BusinessType,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                MerchantsGroup = group
            };

             merchant = await _merchantRepo.AddMerchantAsync(merchant);

            _cache.Remove(AllMerchantsKey);
            _cache.Remove(ByGroupKeyPrefix + groupId);
            _cache.Remove("groups:id:" + groupId);

            return new MerchantDTO
            {
                Id = merchant.Id,
                Name = merchant.Name,
                ManagerName = merchant.ManagerName,
                BusinessType = merchant.BusinessType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = merchant.Status,
            };
        }

        public async Task<MerchantDTO?> UpdateMerchant(long merchantId, MerchantRequest request)
        {
            var merchant = await _merchantRepo.GetMerchantByIdAsync(merchantId);
            if (merchant == null) return null;

            merchant.ManagerName = request.ManagerName;
            merchant.BusinessType = request.BusinessType;
            merchant.Status = request.Status;
            merchant.UpdatedAt = DateTime.UtcNow;
            merchant.Name = request.Name;

            merchant = await _merchantRepo.UpdateMerchantAsync(merchant);

            _cache.Remove(MerchantByIdPrefix + merchantId);
            _cache.Remove(AllMerchantsKey);
            _cache.Remove("groups:id:" + merchant.MerchantsGroup.Id);

            return new MerchantDTO
            {
                Id = merchant.Id,
                Name = merchant.Name,
                ManagerName = merchant.ManagerName,
                BusinessType = merchant.BusinessType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = merchant.Status,
            };
        }

        public async Task<MerchantDTO?> ChangeMerchantGroup(long merchantId, long newGroupId)
        {
            var merchant = await _merchantRepo.GetMerchantByIdAsync(merchantId);
            var newGroup = await _groupRepo.GetGroupByIdAsync(newGroupId);

            if (merchant == null || newGroup == null) return null;

            merchant.MerchantsGroup = newGroup;
            merchant = await _merchantRepo.UpdateMerchantAsync(merchant);

            _cache.Remove("groups:id:" + merchant.MerchantsGroup.Id);
            _cache.Remove("groups:id:" + newGroupId);

            return new MerchantDTO
            {
                Id = merchant.Id,
                Name = merchant.Name,
                ManagerName = merchant.ManagerName,
                BusinessType = merchant.BusinessType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = merchant.Status,
            };
        }

        public async Task<MerchantDTO?> GetMerchantWithMainBranch(long merchantId)
        {
            var merchant = await _merchantRepo.GetMerchantWithMainBranch(merchantId);
            return merchant != null ? new MerchantDTO
            {
                Id = merchant.Id,
                Name = merchant.Name,
                ManagerName = merchant.ManagerName,
                BusinessType = merchant.BusinessType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = merchant.Status,
                MerchantBranches = merchant.MerchantBranches.Select(branch => new MerchantBranchDto{
                    Id = branch.Id,
                    BranchName = branch.BranchName,
                    Status = branch.Status,
                    IsMainBranch = branch.IsMainBranch,
                    Address = branch.Address,
                    Mobile = branch.Mobile,
                    Phone = branch.Phone,
                }).ToList()
            } : null;
        }

        public async Task<List<MerchantDTO>> SearchMerchants(SearchRequest request)
        {
            var merchants = await _merchantRepo.SearchMerchants(request);
            
            return merchants.Select(merchant => new MerchantDTO
            {
                Id = merchant.Id,
                Name = merchant.Name,
                ManagerName = merchant.ManagerName,
                BusinessType = merchant.BusinessType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = merchant.Status,
            }).ToList();
        }
    }
}
