namespace english_learning_server.Dtos.Profile.Response
{
    public class UploadImageResponseDto
    {
        public bool Success => true;
        public string AuthenticatedUrl { get; set; } = string.Empty;
        public string PublicUrl { get; set; } = string.Empty;
    }
}
