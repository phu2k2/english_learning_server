using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Game.Command
{
    public class TranslateVoiceCommandDto
    {
        [Required]
        public string WavFileGcsUri { get; set; } = null!;
    }
}
