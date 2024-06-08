namespace english_learning_server.Failure
{
    public class ApiErrorResponse
    {
        public bool Success => false;
        public string Message { get; set; } = string.Empty;
    }
}
