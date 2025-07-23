using CommonLib.Interfaces;
using CommonLib.Localization;
using CommonLib.Models;
using CommonLib.RequestBody;
using CommonLib.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiLib.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly IMerchantBranchService _branchService;

        public BranchController(IMerchantBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MerchantBranches>>> GetAllBranches()
        {
            var branches = await _branchService.GetAllBranchesAsync();
            return Ok(branches);
        }

        [HttpPost("{merchantId}")]
        public async Task<ActionResult<MerchantBranches>> AddBranch(long merchantId, [FromBody] BranchRequest branchBody)
        {
            var newBranch = await _branchService.AddBranchAsync(branchBody, merchantId);

            if (newBranch == null)
            {
               var ErrorResponse = new ResponseMessage
               {
                    Details = null,
                    Message = LocalizedMessage.GetMessage("NotFound", "merchant", merchantId),
                    StatusCode = ResponseCode.NotFound,
                };
                return NotFound(ErrorResponse);
            }

            return CreatedAtAction(nameof(GetAllBranches), new { id = newBranch.Id }, newBranch);
        }
    }
}
