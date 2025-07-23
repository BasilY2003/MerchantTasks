namespace CommonLib.Localization
{
    public class ResponseMessage
    {
        public ResponseCode StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Details { get; set; }
    }
}
