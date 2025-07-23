using CommonLib.Interfaces;
using CommonLib.Localization;
using CommonLib.Middlewares;
using CommonLib.Models;
using CommonLib.Pdf;
using CommonLib.RequestBody;
using CommonLib.Services;
using Microsoft.AspNetCore.Mvc;


namespace ApiLib.Controllers
{
    [ApiController]
    [Route("api/groups")]
    [ServiceFilter(typeof(JwtAuthFilter))]
    public class MerchantGroupController : BaseController
    {
        private readonly IMerchantGroupService _service;
        private readonly PdfGenerator _pdfGenerator;

        public MerchantGroupController(IMerchantGroupService service, PdfGenerator pdfGenerator)
        {
            _service = service;
            _pdfGenerator = pdfGenerator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(long id)
        {
            var group = await _service.GetGroupWithMerchantsById(id);

            if (group == null) return NotFoundResponse("Group", id);
            return Ok(group);
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GenerateGroupPdf(long id)
        {
            var group = await _service.GetGroupWithMerchantsById(id);

            if (group == null) return NotFoundResponse("Group", id);
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
            
            if (updated == null) return NotFoundResponse("Group", id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(long id)
        {
            var deleted = await _service.DeleteGroupAsync(id);
        
            if (!deleted) return NotFoundResponse("Group", id);
            return NoContent();
        }
    }
}
