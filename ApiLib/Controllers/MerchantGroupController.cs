using CommonLib.Interfaces;
using CommonLib.Middlewares;
using CommonLib.Pdf;
using CommonLib.RequestBody;
using DataLib.RequestBody;
using Microsoft.AspNetCore.Mvc;


namespace ApiLib.Controllers
{
    [ApiController]
    [Route("api/groups")]
    //[ServiceFilter(typeof(JwtAuthFilter))]
    public class MerchantGroupController : BaseController
    {
        private readonly IMerchantGroupService _service;
        private readonly PdfGenerator _pdfGenerator;
        private readonly IAesEncryptionService _aesEncryptionService;

        public MerchantGroupController(IMerchantGroupService service, PdfGenerator pdfGenerator, IAesEncryptionService aesEncryptionService)
        {
            _service = service;
            _pdfGenerator = pdfGenerator;
            _aesEncryptionService = aesEncryptionService;
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

        [HttpGet("encryption")]
        public async Task<IActionResult> TestAdvancedEncryptionStandard()
        {
            string str = "This is the encrypted String";
            var (encryptedData, key, iv) = _aesEncryptionService.Encrypt(str);
            var original = _aesEncryptionService.Decrypt(encryptedData, key,iv);
           
            return Ok(new AesRequest{
              EncryptedMessage = encryptedData,
              Key = key,
              Iv = iv,
            });
        }

        [HttpPost("decryption")]
        public async Task<IActionResult> TestDycryption([FromBody] AesRequest body1)
        {
            var original = _aesEncryptionService.Decrypt(body1.EncryptedMessage,body1.Key,body1.Iv);

            return Ok(original);
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
