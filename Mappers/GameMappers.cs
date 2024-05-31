using english_learning_server.Dtos.Game;
using english_learning_server.Dtos.Option;
using english_learning_server.Models;

namespace english_learning_server.Mappers
{
    public static class GameMappers
    {
        public static GameDto ToGameDto(this Game gameModel, Guid topicId)
        {
            return new GameDto
            {
                Id = gameModel.Id,
                Kind = gameModel.Kind,
                Question = gameModel.Question,
                RightAnswer = gameModel.RightAnswer,
                SoundFilePath = gameModel.SoundFilePath,
                TopicId = topicId,

                options = gameModel.Options.Select(o => new OptionDto
                {
                    Id = o.Id,
                    GameId = gameModel.Id,
                    Name = o.Name
                }).ToList()
            };
        }

        public static GameTopicDto ToGameTopicDto(this Game gameModel, Guid topicId)
        {
            return new GameTopicDto
            {
                GameId = gameModel.Id,
                TopicId = topicId
            };
        }
    }
}