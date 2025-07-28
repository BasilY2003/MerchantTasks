namespace CommonLib.RequestBody
{
    public class AesRequest
    {
       public byte[] EncryptedMessage { get; set; }
       public byte[] Key { get; set; }
       public byte[] Iv { get; set; }
    }
}
