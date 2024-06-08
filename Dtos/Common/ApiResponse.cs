namespace english_learning_server.Dtos.Common
{
    public class ApiResponse
    {
        public bool Success => true;
        public string Message { get; set; } = string.Empty;
    }
}
