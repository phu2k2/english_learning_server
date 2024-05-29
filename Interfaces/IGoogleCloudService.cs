namespace english_learning_server.Interfaces
{
    public class AvatarUrl
    {
        public string PublicUrl { get; set; }
        public string AuthenticatedUrl { get; set; }
    }

    public interface IGoogleCloudService
    {
        public Task<string> TransSpeechToText(string wavFileGcsUri);

        public Task<string> UploadFileToBucket(string fileName, Stream fileStream, string contentType);

        public AvatarUrl GetPublicAndAuthenticatedUrl(string fileName);        
    }
}
