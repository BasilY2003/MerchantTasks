namespace CommonLib.Middlewares
{
    public class TestException : Exception
    {
        public TestException() : base("A test exception occurred.")
        {
        }

        public TestException(string message) : base(message)
        {
        }

        public TestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
