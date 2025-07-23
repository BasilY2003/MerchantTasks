namespace CommonLib.Localization
{
    public class ErrorResponse
    {
        public ErrorCode StatusCode { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
        public object? Details { get; set; }
    }
}
