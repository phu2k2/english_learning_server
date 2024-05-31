using english_learning_server.Dtos.Game;
using english_learning_server.Dtos.Option;
using english_learning_server.Models;

namespace english_learning_server.Interfaces
{
    public interface ILangChainService
    {
        public Task<List<Game>?> FetchGamesForPictureChoiceByTopic(Guid topicId, string topicName);   

        public Task<List<Game>?> FetchGamesByTopic(Guid topicId, string topicName);

        public Task<List<Option>?> FetchOptionForSentenceScrambleGamesByTopic(List<Game> games, string topicName);

        public Task<List<Option>?> FetchOptionForPictureChoiceByTopic(List<Game> games, string topicName);

        public Task<List<Option>?> FetchOptionForSentenceChoiceByTopic(List<Game> games, string topicName);
    }
}