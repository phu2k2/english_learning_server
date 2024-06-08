namespace english_learning_server.Dtos.ProfileGame.Query
{
    public class GetProfileGamesQueryDto
    {
        public Guid ProfileId { get; set; } = Guid.Empty;

        public Guid TopicId { get; set; } = Guid.Empty;
    }
}
