using System.ComponentModel.DataAnnotations;
using english_learning_server.Dtos.Option;

namespace english_learning_server.Dtos.Game
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public string? Kind { get; set; }
        public string? Question { get; set; }
        public string? RightAnswer { get; set; }
        public string? SoundFilePath { get; set; }
        public Guid TopicId { get; set; }
        public List<OptionDto> options { get; set; } = new List<OptionDto>();
    }

    public class GameTopicDto
    {
        [Required]
        public Guid GameId { get; set; }
        [Required]
        public Guid TopicId { get; set; }
    }
}
