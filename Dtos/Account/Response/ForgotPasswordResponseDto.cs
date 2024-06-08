namespace english_learning_server.Dtos.Account.Response
{
    public class ForgotPasswordResponseDto
    {
        public bool Success => true;
        public string Message => "Email sent successfully";
        public string Token { get; set; } = string.Empty;
    }
}