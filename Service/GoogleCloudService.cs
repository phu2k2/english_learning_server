using english_learning_server.Interfaces;
using Google.Cloud.Speech.V1;
using Google.Cloud.Storage.V1;
using Google.Cloud.TextToSpeech.V1;

namespace english_learning_server.Service
{
    public class GoogleCloudService : IGoogleCloudService
    {
        private readonly SpeechClient _speechClient;
        private readonly StorageClient _storageClient;
        private readonly TextToSpeechClient _textToSpeechClient;
        private readonly string? _bucketName;
        private readonly IConfiguration _config;

        public GoogleCloudService(IConfiguration config)
        {
            _config = config;
            _speechClient = SpeechClient.Create();
            _storageClient = StorageClient.Create();
            _textToSpeechClient = TextToSpeechClient.Create();
            _bucketName = _config["GoogleCloud:BucketName"];
        }

        public async Task<string> TransSpeechToText(string wavFileGcsUri)
        {
            var response = await _speechClient.RecognizeAsync(new RecognitionConfig
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                LanguageCode = "en-US",
                ProfanityFilter = false,
                SampleRateHertz = 16000
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

        public async Task<byte[]> SynthesizeSpeech(string text)
        {
            var input = new SynthesisInput
            {
                Text = text
            };

            var voiceSelection = new VoiceSelectionParams
            {
                LanguageCode = "en-US",
                Name = "en-US-Standard-F",
            };

            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };

            var response = await _textToSpeechClient.SynthesizeSpeechAsync(input, voiceSelection, audioConfig);

            return response.AudioContent.ToByteArray();
        }

        public async Task<string> EmailHtmlMessage(string otp, string createdAt, string userName)
        {
            using var memoryStream = new MemoryStream();
            await _storageClient.DownloadObjectAsync(_bucketName, "StaticContent/email.html", memoryStream);
            memoryStream.Position = 0;

            using var streamReader = new StreamReader(memoryStream);
            var htmlMessage = await streamReader.ReadToEndAsync();

            htmlMessage = htmlMessage.Replace("{{ otp }}", otp);
            htmlMessage = htmlMessage.Replace("{{ createdAt }}", createdAt);
            htmlMessage = htmlMessage.Replace("{{ userName }}", userName);

            return htmlMessage;
        }
    }
}
