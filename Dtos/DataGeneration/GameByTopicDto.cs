using english_learning_server.Dtos.Game;

namespace english_learning_server.Dtos.DataGeneration
{
    public class GameByTopicDto
    {
        public IEnumerable<CreateGameDto> Games { get; set; } = new List<CreateGameDto>();
    }
}
