using english_learning_server.Dtos.Option;

namespace english_learning_server.Dtos.Game.Response
{
    public class GetGamesResponseDto
    {
        public IEnumerable<GameDto> Games { get; set; } = new List<GameDto>();
    }

    public class GameDto
    {
        public Guid Id { get; set; }
        public string? Kind { get; set; }
        public string? Question { get; set; }
        public string? RightAnswer { get; set; }
        public Guid TopicId { get; set; }
        public List<OptionDto> options { get; set; } = new List<OptionDto>();
    }
}
