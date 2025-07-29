namespace CommonLib.RequestBody
{
    public class AesRequest
    {
       public byte[] EncryptedMessage { get; set; }
       public string Key { get; set; }
       public string Iv { get; set; }
    }

  
}
