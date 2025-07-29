using CommonLib.Interfaces;
using CommonLib.Middlewares;
using CommonLib.Pdf;
using CommonLib.RequestBody;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


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
        private readonly IUserService _userService;

        public MerchantGroupController(IMerchantGroupService service, PdfGenerator pdfGenerator, IAesEncryptionService aesEncryptionService, IRsaKeyService rsaKeyService,IUserService userService)
        {
            _service = service;
            _pdfGenerator = pdfGenerator;
            _aesEncryptionService = aesEncryptionService;
            _rsaKeyService = rsaKeyService;
            _userService = userService;
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
        public async Task<IActionResult> TestAdvancedEncryptionStandard([FromBody] RsaEncryptRequest request)
        {
            string str = "This is the encrypted String With RSA and AES";

            // Encrypt the message and return the Key and the IV 
            var (encryptedData, key, iv) = _aesEncryptionService.Encrypt(str);

            var userNameClaim = User.FindFirst(ClaimTypes.Name)?.Value;
            //var u = await _userService.GetByUsernameAsync(userNameClaim);

            var publicKey = request.PublicKey;

            // Encrypt the IV and the Key using the recipient's public key
            var enKey = _rsaKeyService.EncryptWithPublicKey(Convert.ToBase64String(key), publicKey);
            var enIv = _rsaKeyService.EncryptWithPublicKey(Convert.ToBase64String(iv), publicKey);

            return Ok(new AesRequest
            {
                EncryptedMessage = encryptedData,
                Key = Convert.ToBase64String(enKey), 
                Iv = Convert.ToBase64String(enIv)   
            });
        }

        [HttpPost("decryption")]
        public async Task<IActionResult> TestDycryption([FromBody] AesRequest body1)
        {
            byte[] keyBytes = Convert.FromBase64String(body1.Key);
            byte[] ivBytes = Convert.FromBase64String(body1.Iv);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyApp", "keys", userIdClaim);
            var privateKeyPath = Path.Combine(folderPath, "private.pem");
            string privateKeyPem = await System.IO.File.ReadAllTextAsync(privateKeyPath);

            string decryptedKeyString = _rsaKeyService.DecryptWithPrivateKey(keyBytes, privateKeyPem);
            string decryptedIvString = _rsaKeyService.DecryptWithPrivateKey(ivBytes, privateKeyPem);

            // Convert string back to byte[] (if string is base64)
            byte[] decryptedKey = Convert.FromBase64String(decryptedKeyString);
            byte[] decryptedIv = Convert.FromBase64String(decryptedIvString);

            var original = _aesEncryptionService.Decrypt(body1.EncryptedMessage, decryptedKey, decryptedIv);

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
