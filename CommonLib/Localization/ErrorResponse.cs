namespace CommonLib.Localization
{
    public class ErrorResponse
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string? Details { get; set; }
    }
}
