namespace english_learning_server.Dtos.Game.Command
{
    public class UploadVoiceOfUserCommandDto
    {
        public IFormFile audioFile { get; set; } = null!;
    }
}