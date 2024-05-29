

using english_learning_server.Interfaces;
using LangChain.Providers.Google;
using LangChain.Providers.Google.Predefined;

namespace english_learning_server.Service
{
    public class LangChainService : ILangChainService
    {
        private readonly GoogleProvider _provider;
        private readonly IConfiguration _config;
        private readonly GeminiProModel _model;

        public LangChainService(IConfiguration config)
        {
            _config = config;
            _provider = new GoogleProvider(_config["ApiKey:Gemini"], new HttpClient());
            _model = new GeminiProModel(_provider);
        }

        /// <summary>
        /// Generate sentence by topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public async Task<string> GenerateSentenceByTopic(string topic)
        {

            var response = await _model.GenerateAsync(
                $@"
                I'm an AI language model and I'm here to help you learn English. To assist you, I'm going to generate 12 simple English sentences that are related to the topic of {topic}
                . These sentences will be basic and easy to understand, making them perfect for beginners who are just starting to learn English.
                ", cancellationToken: CancellationToken.None).ConfigureAwait(false);

            return response;
        }

        // public async Task<string> GenerateSentenceScrambleGame(string sentence)
        // {
        //     var response = await _model.GenerateAsync(
        //         $"""
        //         Use the following pieces of context to answer the question at the end.
        //         If the answer is not in context then just say that you don't know, don't try to make up an answer.
        //         Keep the answer as short as possible.

        //         {text}

        //         Helpful Answer:
        //         """, cancellationToken: CancellationToken.None).ConfigureAwait(false);

        //     return response;
        // }
    }
}