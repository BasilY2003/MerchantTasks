namespace CommonLib.Localization
{
    public class ErrorsResponse
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public List<string>? Details { get; set; }
    }
}
