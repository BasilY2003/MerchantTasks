using CommonLib.DTOs;
using CommonLib.Interfaces;
using CommonLib.Middlewares;
using CommonLib.RequestBody;
using DataLib.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ApiLib.Controllers
{
    [ApiController]
    [Route("api/merchants")]
    [ServiceFilter(typeof(JwtAuthFilter))]
    public class MerchantController : BaseController
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MerchantDTO>> GetByIdWithBranches(long id)
        {
            var merchant = await _merchantService.GetByIdWithGroupAsync(id);
            if (merchant == null)
            {
                NotFoundResponse("merchant",id);
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
                NotFoundResponse("Group", groupId);
            }
            return Ok(merchants);
        }

        [HttpPut("{merchantId}/group/{newGroupId}")]
        public ActionResult<MerchantDTO> ChangeMerchantGroup(long merchantId, long newGroupId)
        {
            var updatedMerchant = _merchantService.ChangeMerchantGroup(merchantId, newGroupId);
            if (updatedMerchant == null)
            {
                NotFoundResponse("merchant", merchantId);
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
                NotFoundResponse("Group", groupId);
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
                NotFoundResponse("merchant", merchantId);
            }
            return Ok(updated);
        }
    
        [HttpGet("{merchantId}/main-branch")]
        public async Task<ActionResult<MerchantDTO>> GetMerchantWithMainBranch(long merchantId)
        {
            var merchant = await _merchantService.GetMerchantWithMainBranch(merchantId);
            
            if (merchant == null) return NotFoundResponse("merchant", merchantId);
            return Ok(merchant);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<MerchantDTO>>> Search([FromQuery] SearchRequest request)
        {
            var merchants = await _merchantService.SearchMerchants(request);
            return Ok(merchants);
        }
    }
}
