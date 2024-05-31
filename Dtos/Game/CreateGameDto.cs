using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Game
{
    public class CreateGameDto
    {
        [Required]
        public string Kind { get; set; } = string.Empty;
        public string? Question { get; set; }
        [Required]
        public string RightAnswer { get; set; } = string.Empty;
        [Required]
        public Guid TopicId { get; set; }
    }
}
