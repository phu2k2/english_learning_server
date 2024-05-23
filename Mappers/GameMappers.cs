using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using english_learning_server.Dtos.Game;
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
                TopicId = topicId
            };
        }
    }
}