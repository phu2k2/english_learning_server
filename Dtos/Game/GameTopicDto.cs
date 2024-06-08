using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Game
{
    public class GameTopicDto
    {
        [Required]
        public Guid GameId { get; set; }
        [Required]
        public Guid TopicId { get; set; }
    }
}
