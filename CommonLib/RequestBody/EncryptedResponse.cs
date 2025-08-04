namespace CommonLib.RequestBody
{
    internal class EncryptedResponse
    {
        public string EncryptedMessage { get; set; }
        public string Key { get; set; }
        public string Iv { get; set; }   
    }
}
