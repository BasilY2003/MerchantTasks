namespace CommonLib.RequestBody
{
    public class Email
    {
        public string SmtpServer { get; set; } = String.Empty;
        public int Port { get; set; }
        public string SenderEmail { get; set; } = String.Empty;
        public string SenderName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
