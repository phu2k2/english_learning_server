using english_learning_server.Dtos.Topic;
using english_learning_server.Dtos.Topic.Response;
using english_learning_server.Models;

namespace english_learning_server.Mappers
{
    public static class TopicMappers
    {
        public static GetTopicsResponseDto ToTopicsResponseDto(this IEnumerable<Topic> topicModels, Guid profileId)
        {
            return new GetTopicsResponseDto
            {
                Topics = topicModels.Select(s => new TopicDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Image = s.Image,
                    NumberOfGame = s.Games.Count,
                    NumberOfPlayedGames = s.Games.Count(g => g.ProfileGames.Any(pg => pg.ProfileId == profileId && pg.IsPlayed)),
                })
            };
        }
    }
}
