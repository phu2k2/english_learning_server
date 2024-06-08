namespace english_learning_server.Dtos.Game.Response
{
    public class TranslateVoiceResponseDto
    {
        public bool Success => true;
        public string Text { get; set; } = string.Empty;
    }
}