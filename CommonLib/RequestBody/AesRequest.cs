using System.Text.Json.Serialization;

namespace CommonLib.RequestBody
{
    public class AesRequest
    {

        [JsonPropertyName("encryptedMessage")]
        public string EncryptedMessage { get; set; }
       
        [JsonPropertyName("keyWithIv")]
        public string KeyWithIv { get; set; }
    }
}
