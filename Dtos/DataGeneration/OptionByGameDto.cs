using english_learning_server.Dtos.Game;

namespace english_learning_server.Dtos.DataGeneration
{
    public class OptionByGameDto
    {
        public IEnumerable<CreateGameDto> Options { get; set; } = new List<CreateGameDto>();
    }
}
