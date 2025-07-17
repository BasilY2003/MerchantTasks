using CommonLib.DTOs;
using CommonLib.Localization;
using CommonLib.Middlewares;
using CommonLib.RequestBody;
using CommonLib.Services;
using DataLib.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ApiLib.Controllers
{
    [ApiController]
    [Route("api/merchants")]
    [ServiceFilter(typeof(JwtAuthFilter))]
    public class MerchantController : ControllerBase
    {
        private readonly MerchantService _merchantService;

        public MerchantController(MerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MerchantDTO>> GetByIdWithBranches(long id)
        {
            var merchant = await _merchantService.GetByIdWithGroupAsync(id);
            if (merchant == null)
            {
                var error = LocalizedErrorHelper.Create(ErrorCode.NotFound, "NotFound", "Merchant", id);
                return NotFound(error);
            }
            return Ok(merchant);
        }

        [HttpGet]
        public async Task<ActionResult<List<MerchantDTO>>> GetAll()
        {
            var merchants = await _merchantService.GetAllAsync();
            return Ok(merchants);
        }

        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<List<MerchantDTO>>> GetByGroupId(long groupId)
        {
            var merchants = await _merchantService.GetMerchantsByGroupIdAsync(groupId);

            if (merchants == null)
            {
                var error = LocalizedErrorHelper.Create(ErrorCode.NotFound, "NotFound", "Group", groupId);
                return NotFound(error);
            }
            return Ok(merchants);
        }

        [HttpPut("{merchantId}/group/{newGroupId}")]
        public ActionResult<MerchantDTO> ChangeMerchantGroup(long merchantId, long newGroupId)
        {
            var updatedMerchant = _merchantService.ChangeMerchantGroup(merchantId, newGroupId);
            if (updatedMerchant == null)
            {
                var error = LocalizedErrorHelper.Create(ErrorCode.NotFound, "NotFound", "Merchant", merchantId);
                return NotFound(error);
            }
            return Ok(updatedMerchant);
        }

        [HttpPost("{groupId}")]
        public async Task<ActionResult<MerchantDTO>> AddMerchant(long groupId, [FromBody] MerchantRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var merchant = await _merchantService.AddMerchant(request, groupId);
            if (merchant == null)
            {
                var error = LocalizedErrorHelper.Create(ErrorCode.NotFound, "NotFound", "Group", groupId);
                return NotFound(error);
            }
            return CreatedAtAction(nameof(GetByIdWithBranches), new { id = merchant.Id }, merchant);
        }

        [HttpPut("{merchantId}")]
        public async Task<ActionResult<MerchantDTO>> UpdateMerchant(long merchantId, [FromBody] MerchantRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _merchantService.UpdateMerchant(merchantId, request);
            if (updated == null)
            {
                var error = LocalizedErrorHelper.Create(ErrorCode.NotFound, "NotFound", "Merchant", merchantId);
                return NotFound(error);
            }
            return Ok(updated);
        }

    
        [HttpGet("{merchantId}/main-branch")]
        public async Task<ActionResult<MerchantDTO>> GetMerchantWithMainBranch(long merchantId)
        {
            var merchant = await _merchantService.GetMerchantWithMainBranch(merchantId);
            
            if (merchant == null) return NotFound("Merchant with main branch not found.");
            return Ok(merchant);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<MerchantDTO>>> Search([FromQuery] SearchRequest request)
        {
            var merchants = await _merchantService.SearchMerchants(request);
            return Ok(merchants);
        }

        //[HttpPost("search-with-branches")]
        //public async Task<ActionResult<List<MerchantDTO>>> SearchWithBranches([FromBody] SearchRequest request)
        //{
        //    var merchants = await _merchantService.SearchMerchantsWithBranches(request);
        //    return Ok(merchants);
        //}


        [HttpGet("test-locale")]
        public string Test([FromServices] IStringLocalizer<SharedResource> localizer)
        {
            return localizer["Required", "Name"];
        }
    }
}
