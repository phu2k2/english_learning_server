using Microsoft.AspNetCore.Mvc;

namespace english_learning_server.Dtos.Game.Response
{
    public class SynthesizeSpeechResponseDto
    {
        public FileStreamResult FileStreamResult { get; set; } = null!;
    }
}