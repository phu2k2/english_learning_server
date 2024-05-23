using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Topic
{
    public class TopicDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Image { get; set; } = null!;

        public int NumberOfGame { get; set; }
    }
}
