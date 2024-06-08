namespace english_learning_server.Dtos.Topic.Response
{
    public class GetTopicsResponseDto
    {
        public IEnumerable<TopicDto> Topics { get; set; } = new List<TopicDto>();
    }

    public class TopicDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int NumberOfGame { get; set; }
        public int NumberOfPlayedGames { get; set; }
    }
}