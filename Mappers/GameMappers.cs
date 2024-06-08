using english_learning_server.Dtos.Option;
using english_learning_server.Dtos.ProfileGame.Response;
using english_learning_server.Models;

namespace english_learning_server.Mappers
{
    public static class GameMappers
    {
        public static GetProfileGamesResponseDto ToProfileGamesResponseDto(this IEnumerable<ProfileGame> profileGamesModel, Guid topicId)
        {
            return new GetProfileGamesResponseDto
            {
                Games = profileGamesModel.Select(g => new ProfileGameDto
                {
                    Id = g.Game.Id,
                    Kind = g.Game.Kind,
                    Question = g.Game.Question,
                    RightAnswer = g.Game.RightAnswer,
                    SoundFilePath = g.Game.SoundFilePath,
                    TopicId = topicId,
                    IsPlayed = g.IsPlayed,
                    options = g.Game.Options.Select(o => new OptionDto
                    {
                        Id = o.Id,
                        GameId = o.GameId,
                        Name = o.Name,
                        PhotoFilePath = o.PhotoFilePath
                    }).ToList()
                })
            };
        }
    }
}
