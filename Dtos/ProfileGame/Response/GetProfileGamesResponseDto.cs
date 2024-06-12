using english_learning_server.Dtos.Option;

namespace english_learning_server.Dtos.ProfileGame.Response
{
    public class GetProfileGamesResponseDto
    {
        public IEnumerable<ProfileGameDto> Games { get; set; } = new List<ProfileGameDto>();
    }

    public class ProfileGameDto
    {
        public Guid Id { get; set; }
        public string? Kind { get; set; }
        public string? Question { get; set; }
        public string? RightAnswer { get; set; }
        public Guid TopicId { get; set; }
        public bool? IsPlayed {get; set;}
        public List<OptionDto> options { get; set; } = new List<OptionDto>();
    }
}
