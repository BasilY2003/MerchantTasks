using CommonLib.Interfaces;
using CommonLib.Middlewares;
using CommonLib.Pdf;
using CommonLib.RequestBody;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using DataLib.Interfaces;


namespace ApiLib.Controllers
{
    [ApiController]
    [Route("api/groups")]
    [ServiceFilter(typeof(JwtAuthFilter))]
    public class MerchantGroupController : BaseController
    {
        private readonly IMerchantGroupService _service;
        private readonly PdfGenerator _pdfGenerator;
        private readonly IAesEncryptionService _aesEncryptionService;
        private readonly IRsaKeyService _rsaKeyService;
        private readonly IServerKeyRepository _serverKey;

        public MerchantGroupController(IMerchantGroupService service, PdfGenerator pdfGenerator,IServerKeyRepository keyRepository,IRsaKeyService rsaKeyService,IAesEncryptionService aesEncryptionService)
        {
            _service = service;
            _pdfGenerator = pdfGenerator;
            _aesEncryptionService = aesEncryptionService;
            _rsaKeyService = rsaKeyService;
            _serverKey = keyRepository;
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

        [HttpPost("encryption")]
        public async Task<IActionResult> TestAdvancedEncryptionStandard([FromBody] Object req)
        {

            string jsonString = JsonSerializer.Serialize(req);

            var (encryptedData, key) = _aesEncryptionService.Encrypt(jsonString);

            var Keys = await _serverKey.GetAsync();
            var publicKey = Keys.PublicKey;
            var enKey = _rsaKeyService.EncryptWithPublicKey(key, publicKey);
           //  var enIv = _rsaKeyService.EncryptWithPublicKey(iv, publicKey);

            return Ok(new AesRequest
            {
                EncryptedMessage = Convert.ToBase64String(encryptedData),
                KeyWithIv = Convert.ToBase64String(enKey),
              //  Iv = Convert.ToBase64String(enIv)
            });
        }

        [HttpPost("decryption")]
        public async Task<IActionResult> TestDycryption([FromBody] AesRequest body1)
        {
            byte[] keyBytes = Convert.FromBase64String(body1.KeyWithIv);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApp", "keys", userIdClaim);
            var privateKeyPath = Path.Combine(folderPath, "private.pem");
            string privateKeyPem = await System.IO.File.ReadAllTextAsync(privateKeyPath);

           var decryptedKey = _rsaKeyService.DecryptWithPrivateKey(keyBytes, privateKeyPem);
           var str = Convert.FromBase64String(body1.EncryptedMessage);
            var original = _aesEncryptionService.Decrypt(str, decryptedKey);

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
            var user = HttpContext.User;
            Console.WriteLine(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
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