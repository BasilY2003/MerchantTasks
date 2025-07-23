using CommonLib.DTOs;
using CommonLib.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using CommonLib.RequestBody;
using DataLib.Interfaces;
using CommonLib.Interfaces;

namespace CommonLib.Services
{
    public class MerchantGroupService : IMerchantGroupService
    {
        private const string AllKeyPrefix = "groups:all";
        private const string ByIdKeyPrefix = "groups:id:";

        private readonly IMerchantGroupRepository _repository;
        
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };

        public MerchantGroupService(IMerchantGroupRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
            _cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
        }

        public async Task<MerchantGroupDto?> GetGroupWithMerchantsById(long id)
        {
            var cacheKey = ByIdKeyPrefix + id;
            MerchantsGroups? group = null;

            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                Console.WriteLine($"Cache HIT for Group Entity By ID {id}");
                group = JsonSerializer.Deserialize<MerchantsGroups>(cached, _jsonOptions);
            }
            else
            {
                group = await _repository.GetGroupWithMerchantsByIdAsync(id);
                if (group != null)
                {
                    await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(group, _jsonOptions), _cacheOptions);
                }
            }
            if (group == null) return null;

            return new MerchantGroupDto
            {
                Name = group.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Id = id,
                Merchants = group.Merchants.Select(merchant => new MerchantDTO
                {
                    Id = merchant.Id,
                    Name = merchant.Name,
                    BusinessType = merchant.BusinessType,
                    Status = merchant.Status,
                    ManagerName = merchant.ManagerName,

                }).ToList()
            }; 
        }

        public async Task<List<MerchantGroupDto>> GetAllGroupsAsync()
        {
            var cacheKey = AllKeyPrefix;
            List<MerchantsGroups>? groups = null;

            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                Console.WriteLine("Cache HIT for All Group Entities");
                groups = JsonSerializer.Deserialize<List<MerchantsGroups>>(cached, _jsonOptions);
            }
            else
            {
                groups = await _repository.GetAllAsync();
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(groups, _jsonOptions), _cacheOptions);
            }

            return groups.Select(group => new MerchantGroupDto
            {
                Id = group.Id,
                CreatedAt = group.CreatedAt,
                UpdatedAt = group.UpdatedAt,
                Name = group.Name,
            }).ToList();
        }

        public async Task<MerchantGroupDto> CreateGroupAsync(MerchantGroupRequest request)
        {
            var date = DateTime.UtcNow;
            var group = new MerchantsGroups
            {
                Name = request.Name,
                CreatedAt = date,
                UpdatedAt = date,
            };

            group = await _repository.CreateGroupAsync(group);

            var cacheKey = ByIdKeyPrefix + group.Id;
            var cachedData = JsonSerializer.Serialize(group, _jsonOptions);
            await _cache.RemoveAsync(AllKeyPrefix).ConfigureAwait(false);
            await _cache.SetStringAsync(cacheKey, cachedData , _cacheOptions).ConfigureAwait(false);

            return new MerchantGroupDto
            {
                Id = group.Id,
                CreatedAt = group.CreatedAt,
                UpdatedAt = group.UpdatedAt,
                Name = group.Name,
            };
        }

        public async Task<MerchantGroupDto?> UpdateGroupAsync(MerchantGroupRequest request, long groupId)
        {
            var group = await _repository.GetGroupByIdAsync(groupId);
            if (group == null) return null;

            group.Name = request.Name;
            group.UpdatedAt = DateTime.UtcNow;

            var cacheKey = ByIdKeyPrefix + group.Id;
            await _repository.UpdateGroupAsync(group);
            await _cache.RemoveAsync(AllKeyPrefix);
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(group, _jsonOptions), _cacheOptions);

            return new MerchantGroupDto
            {
                Id = group.Id,
                CreatedAt = group.CreatedAt,
                UpdatedAt = group.UpdatedAt,
                Name = group.Name,
            };
        }

        public async Task<bool> DeleteGroupAsync(long groupId)
        {
            var group = await _repository.GetGroupByIdAsync(groupId);
            if (group == null) return false;

            var deleted = await _repository.DeleteGroupAsync(group);
            if (!deleted) return false;

            await _cache.RemoveAsync(AllKeyPrefix);
            await _cache.RemoveAsync(ByIdKeyPrefix + groupId);

            return true;
        }
    }
}
