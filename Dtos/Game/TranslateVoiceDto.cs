using System.ComponentModel.DataAnnotations;
using english_learning_server.Dtos.Option;

namespace english_learning_server.Dtos.Game
{
    public class TranslateVoiceDto
    {
        [Required]
        public string WavFileGcsUri { get; set; } = null!;
    }
}
