using CommonLib.Models;
using CommonLib.RequestBody;
using CommonLib.Services;
using DataLib.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace ProjectUnitTests.Tests
{
    public class MerchantGroupTests
    {

        private readonly Mock<IMerchantGroupRepository> _repository;
        private readonly Mock<IDistributedCache> _cache;
        private readonly MerchantGroupService _merchantGroupService;

        public MerchantGroupTests()
        {
            _repository = new Mock<IMerchantGroupRepository>();
            _cache = new Mock<IDistributedCache>();

            _merchantGroupService = new MerchantGroupService(_repository.Object,_cache.Object);
        }

        [Fact]
        public async Task GetGroupById_ShouldReturnNull_WhenNotFound()
        {
            _cache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync((byte[]?)null);

            _repository.Setup(repo => repo.GetGroupWithMerchantsByIdAsync(It.IsAny<long>()))
                  .ReturnsAsync((MerchantsGroups?)null);

            var group = await _merchantGroupService.GetGroupWithMerchantsById(1);
            Assert.Null(group);
        }


        [Fact]
        public async Task CreateGroupAsync_ShouldRemoveAllCacheKey_WhenGroupIsCreated()
        {
            var utcNow = DateTime.UtcNow;
            var request = new MerchantGroupRequest { Name = "Test Group" };

            var createdGroup = new MerchantsGroups
            {
                Id = 1,
                Name = "Basil",
                CreatedAt = utcNow,
                UpdatedAt = utcNow
            };

            _repository.Setup(r => r.CreateGroupAsync(It.IsAny<MerchantsGroups>()))
                    .ReturnsAsync(createdGroup);

            var result = await _merchantGroupService.CreateGroupAsync(request);

            _cache.Verify(c => c.RemoveAsync("groups:all", It.IsAny<CancellationToken>()), Times.Once);

            _cache.Verify(c => c.SetAsync(
                It.Is<string>(str => str == $"groups:id:{createdGroup.Id}"),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(createdGroup.Id, result.Id);
        }
    }
}