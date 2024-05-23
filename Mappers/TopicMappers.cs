using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using english_learning_server.Dtos.Topic;
using english_learning_server.Models;

namespace english_learning_server.Mappers
{
    public static class TopicMappers
    {
        public static TopicDto ToTopicDto(this Topic topicModel)
        {
            return new TopicDto
            {
                Id = topicModel.Id,
                Name = topicModel.Name,
                Image = topicModel.Image,
                NumberOfGame = topicModel.NumberOfGame
            };
        }
    }
}