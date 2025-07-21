using CommonLib.Localization;
using CommonLib.Middlewares;
using CommonLib.RequestBody;
using Microsoft.AspNetCore.Mvc;
using CommonLib.Services;
using CommonLib.Pdf;


namespace ApiLib.Controllers
{
    [ApiController]
    [Route("api/groups")]
    [ServiceFilter(typeof(JwtAuthFilter))]
    public class MerchantGroupController : ControllerBase
    {
        private readonly MerchantGroupService _service;
        private readonly PdfGenerator _pdfGenerator;

        public MerchantGroupController(MerchantGroupService service,PdfGenerator pdfGenerator)
        {
            _service = service;
            _pdfGenerator = pdfGenerator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(long id)
        {
            var group = await _service.GetGroupWithMerchantsById(id);

            if (group == null)
            {
                var errorMessage = LocalizedMessage.GetMessage("NotFound", "Group", id);
                var error = new ErrorResponse
                {
                    ErrorCode = ErrorCode.NotFound,
                    ErrorMessage = errorMessage,
                    Details = null,
                };
                return NotFound(error);
            }
            return Ok(group);
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GenerateGroupPdf(long id)
        {
            var group = await _service.GetGroupWithMerchantsById(id);

            if (group == null)
            {
                var errorMessage = LocalizedMessage.GetMessage("NotFound", "Group", id);
                var error = new ErrorResponse
                {
                    ErrorCode = ErrorCode.NotFound,
                    ErrorMessage = errorMessage,
                    Details = null,
                };
                return NotFound(error);
            }
                _pdfGenerator.GeneratePdf(group);
            return Ok();
        }



        [HttpGet("ex")]
        public IActionResult GetException()
        {
            throw new TestException("Exception made");
        }


        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _service.GetAllGroupsAsync();
            return Ok(groups);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] MerchantGroupRequest request)
        {
            var created = await _service.CreateGroupAsync(request);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(long id, [FromBody] MerchantGroupRequest request)
        {
            var updated = await _service.UpdateGroupAsync(request, id);
            if (updated == null)
            {
                var errorMessage = LocalizedMessage.GetMessage("NotFound", "Group", id);
                var error = new ErrorResponse
                {
                    ErrorCode = ErrorCode.NotFound,
                    ErrorMessage = errorMessage,
                    Details = null,
                };
                return NotFound(error);
            }
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(long id)
        {
            var deleted = await _service.DeleteGroupAsync(id);
            if (!deleted) {
                var errorMessage = LocalizedMessage.GetMessage("NotFound", "Group", id);
                var error = new ErrorResponse
                {
                    ErrorCode = ErrorCode.NotFound,
                    ErrorMessage = errorMessage,
                    Details = null,
                };
                return NotFound(error); }

            return Ok();
        }
    }
}
