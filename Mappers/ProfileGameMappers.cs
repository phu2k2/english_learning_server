using english_learning_server.Dtos.Option;
using english_learning_server.Dtos.ProfileGame.Query;
using english_learning_server.Dtos.ProfileGame.Response;
using english_learning_server.Models;

namespace english_learning_server.Mappers
{
    public static class ProfileGameMappers
    {
        public static GetProfileGamesResponseDto ToProfileGamesResponseDto(this IEnumerable<Game> profileGamesModel, GetProfileGamesQueryDto profileGamesQueryDto)
        {
            return new GetProfileGamesResponseDto
            {
                Games = profileGamesModel.Select(g => new ProfileGameDto
                {
                    Id = g.Id,
                    Kind = g.Kind,
                    Question = g.Question,
                    RightAnswer = g.RightAnswer,    
                    SoundFilePath = g.SoundFilePath,
                    TopicId = g.TopicId,
                    IsPlayed = g.ProfileGames.FirstOrDefault(pg => pg.ProfileId == profileGamesQueryDto.ProfileId)?.IsPlayed,
                    options = g.Options.Select(o => new OptionDto
                    {
                        Id = o.Id,
                        GameId = o.GameId,
                        Name = o.Name
                    }).ToList()
                })
            };
        }
    }
}
