using english_learning_server.Interfaces;
using Google.Cloud.Speech.V1;
using Google.Cloud.Storage.V1;

namespace english_learning_server.Service
{
    public class GoogleCloudService : IGoogleCloudService
    {
        private readonly SpeechClient _speechClient;
        private readonly StorageClient _storageClient;
        private readonly string? _bucketName;
        private readonly IConfiguration _config;

        public GoogleCloudService(IConfiguration config)
        {
            _config = config;
            _speechClient = SpeechClient.Create();
            _storageClient = StorageClient.Create();
            _bucketName = _config["GoogleCloud:BucketName"];
        }

        public async Task<string> TransSpeechToText(string wavFileGcsUri)
        {
            var response = await _speechClient.RecognizeAsync(new RecognitionConfig
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                LanguageCode = "en-US",
                ProfanityFilter = false,
            }, RecognitionAudio.FromStorageUri(wavFileGcsUri));

            string transcribedText = "";

            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    transcribedText += alternative.Transcript;
                }
            }

            return transcribedText;
        }

        public async Task<string> UploadFileToBucket(string fileName, Stream fileStream, string contentType)
        {       
            var objectName = fileName;
            await _storageClient.UploadObjectAsync(_bucketName, objectName, contentType, fileStream);

            var fileGcsUri = $"gs://{_bucketName}/{objectName}";

            return fileGcsUri;
        }

        public AvatarUrl GetPublicAndAuthenticatedUrl(string fileName)
        {
            var urlSigner = UrlSigner.FromServiceAccountPath(_config["GoogleCloud:Credentials"]);

            var expiration = TimeSpan.FromDays(7);

            var url = urlSigner.Sign(_bucketName, fileName, expiration, HttpMethod.Get);

            return new AvatarUrl
            {
                AuthenticatedUrl = url,
                PublicUrl = $"https://storage.googleapis.com/{_bucketName}/{fileName}"
            };
        }        
    }
}
