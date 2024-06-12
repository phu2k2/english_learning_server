using Microsoft.AspNetCore.Mvc;

namespace english_learning_server.Interfaces
{
    public class AvatarUrl
    {
        public string PublicUrl { get; set; } = string.Empty;
        public string AuthenticatedUrl { get; set; } = string.Empty;
    }

    public interface IGoogleCloudService
    {
        public Task<string> TransSpeechToText(string wavFileGcsUri);

        public Task<string> UploadFileToBucket(string fileName, Stream fileStream, string contentType);

        public AvatarUrl GetPublicAndAuthenticatedUrl(string fileName);

        public Task<byte[]> SynthesizeSpeech(string text);
    }
}
