using english_learning_server.Dtos.Game;
using english_learning_server.Dtos.Game.Response;
using english_learning_server.Dtos.Option;
using english_learning_server.Interfaces;
using english_learning_server.Models;
using LangChain.Providers.Google;
using LangChain.Providers.Google.Predefined;
using Newtonsoft.Json;

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

        public async Task<List<Game>?> FetchGamesForPictureChoiceByTopic(Guid topicId, string topicName)
        {
            var response = await _model.GenerateAsync(
                $@"
                I'm an AI language model and I'm here to help you learn English.
                To assist you, I'm going to generate 3 simple English questions and answers for each question for picture choice game that are related to the topic of {topicName}.
                These words will be basic and easy to understand, making them perfect for kids who are just starting to learn English.
                Please create the array[object] have the format:

                    [
                    {{
                        'kind': Picture Choice,
                        'question': string,
                        'rightAnswer' : string,
                        'topicId' : {topicId}
                    }},
                    ...
                    ]

                The topicId of every element is default to {topicId}.
                The kind of each element is enum and default to 'Picture Choice'.
                Just generate this array[object] and do not add any external char like: symbols, punctuation marks, numerical order.
                ", cancellationToken: CancellationToken.None).ConfigureAwait(false);

            var gamesDto = JsonConvert.DeserializeObject<List<GameDto>>(response);

            var games = gamesDto?.Select(x => new Game(x.Kind, x.Question, x.RightAnswer, topicId)).ToList();

            return games;
        }

        public async Task<List<Game>?> FetchGamesByTopic(Guid topicId, string topicName)
        {
            var response = await _model.GenerateAsync(
                $@"
                I'm an AI language model and I'm here to help you learn English.
                To assist you, I'm going to generate 9 English random sentences (not words) for creating game that are related to the topic of {topicName}.
                These sentences will be basic and easy to understand, making them perfect for kids who are just starting to learn English.
                Please create the array[object] have the format:

                    [
                    {{
                        'kind': string
                        'rightAnswer' : string,
                        'topicId' : {topicId}
                    }},
                    ...
                    ]

                The topicId of every element is default to {topicId}.
                The kind of each element is enum and you can random of 3 values : 'Sentence Scramble','Sentence Choice','Echo Repeat'.
                And the sentences is map to the rightAnswer attribute.
                Just generate this array[object] and do not add any external char like: symbols, punctuation marks, numerical order.
                ", cancellationToken: CancellationToken.None).ConfigureAwait(false);

            var gamesDto = JsonConvert.DeserializeObject<List<GameDto>>(response);

            var games = gamesDto?.Select(x => new Game(x.Kind, x.Question, x.RightAnswer, topicId)).ToList();

            return games;
        }

        public async Task<List<Option>?> FetchOptionForSentenceScrambleGamesByTopic(List<Game> games, string topicName)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var gamesJson = JsonConvert.SerializeObject(games, settings);

            var response = await _model.GenerateAsync(
                $@"
                You are an AI language model and You are here to help me generating data for my education English app for kids by game.
                To assist, you are going to generate options for this response.
                Here is the list of 3 games on the {topicName} topic.
                Each element in the list represents one game entity:

                {gamesJson}

                Please focus on the following fields: questions, rightAnswer, and kind of each game object. Based on these fields, generate the options for each game:

                For the 'Sentence Scramble' English game, the options are designed to include parts of the correct answer and distractors.
                There should be a number of options are include parts of the right answer that are scrambled and the number of options are distractors.

                For example if the right answer is 'I am a student', the options could be 'I', 'a', 'student', 'am', 'I teacher', 'I doctor'.
                The rule is I want the number of options (have 1 or 2 words) that are parts of the right answer when arranged in the correct order will be complete the right answer
                And the number of options that are parts of the right answer you can decide, but at most 2 words in one option.
                The options are distractors that are not parts of the right answer and at most 2 words.
                And the number of options that are distractors is 2.

                Please create the list of options (array[object]) in the following format:

                    [
                    {{  
                        'name' : string,
                        'gameId' : Guid
                    }},
                    ...
                    ]

                Ensure that each option object includes:
                - 'name': a string representing the option.
                - 'gameId': the ID field from the corresponding game entity in the games array.

            The important thing I want to mention is that you have to:
                Options have to do not miss any parts of the right answer (any word in the right answer must be in one of the options)!
                Format the response as a JSON array of option objects that can be correctly parsed by JsonConvert.DeserializeObjectm, dont add anything redundant that this lib can not regconize. 
                Do not include any extraneous characters, symbols, punctuation marks, or numerical order.
                ", cancellationToken: CancellationToken.None).ConfigureAwait(false);

            var optionsDto = JsonConvert.DeserializeObject<List<OptionDto>>(response);

            var options = optionsDto?.Select(x => new Option(x.GameId, x.Name)).ToList();

            return options;
        }

        public async Task<List<Option>?> FetchOptionForPictureChoiceByTopic(List<Game> games, string topicName)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var gamesJson = JsonConvert.SerializeObject(games, settings);

            var response = await _model.GenerateAsync(
                $@"
                You are an AI language model and You are here to help me generating data for my education English app for kids by game.
                To assist, you are going to generate 12 options for this response (4 options for each game).
                Here is the list of 3 games on the {topicName} topic.
                Each element in the list represents one game entity:

                {gamesJson}

                Please focus on the following fields: questions, rightAnswer, and kind of each game object. Based on these fields, generate the options for each game:

                For the 'Picture Choice' English game, the system will read a question, the kid will see 4 images for 4 options and choose the correct option that is the right answer.
                So, I want to create 4 options of each game should be random words that, while not the correct answer, is related to the question and the topic: {topicName}.

                Please create the list of options (array[object]) in the following format:

                    [
                    {{  
                        'name' : string,
                        'gameId' : Guid
                    }},
                    ...
                    ]

                Ensure that each option object includes:
                - 'name': a string representing the option.
                - 'gameId': the ID field from the corresponding game entity in the games array.

            The important thing I want to mention is that you have to:
                Generate exactly the required number of options (mean that have to generate 12 objects in this array), without any redundancy.
                Follow the rule of generate 4 options for each game.
                Format the response as a JSON array of option objects that can be correctly parsed by JsonConvert.DeserializeObjectm, dont add anything redundant that this lib can not regconize. 
                Do not include any extraneous characters, symbols, punctuation marks, or numerical order.
                ", cancellationToken: CancellationToken.None).ConfigureAwait(false);

            var optionsDto = JsonConvert.DeserializeObject<List<OptionDto>>(response);

            var options = optionsDto?.Select(x => new Option(x.GameId, x.Name)).ToList();

            return options;
        }

        public async Task<List<Option>?> FetchOptionForSentenceChoiceByTopic(List<Game> games, string topicName)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var gamesJson = JsonConvert.SerializeObject(games, settings);

            var response = await _model.GenerateAsync(
                $@"
                You are an AI language model and You are here to help me generating data for my education English app for kids by game.
                To assist, you are going to generate 12 options for this response (4 options for each game).
                Here is the list of 3 games on the {topicName} topic.
                Each element in the list represents one game entity:

                {gamesJson}

                Please focus on the following fields: questions, rightAnswer, and kind of each game object. Based on these fields, generate the options for each game:

                For the 'Sentence Choice' English game, there will be a sound system to read the answer. The kid will hear the answer and choose the correct option. Each game should have 4 options that are sentences closely similar to the correct answer but incorrect. These options should be designed to make it easy for kid to choose the wrong answer, adding a challenge.

                4 options of each game in above list consists of sentences that are not the correct answer but are closely similar to the question (make the game easy to choose wrongly) and the topic: {topicName}.

                Please create the list of options (array[object]) in the following format:

                    [
                    {{  
                        'name' : string,
                        'gameId' : Guid
                    }},
                    ...
                    ]

                Ensure that each option object includes:
                - 'name': a string representing the option.
                - 'gameId': the ID field from the corresponding game entity in the games array.

            The important thing I want to mention is that you have to:
                Generate exactly the required number of options in this list (mean that have to generate 12 objects in this array), without any redundancy.
                Follow the rule of generate 4 options for each game.
                Format the response as a JSON array of option objects that can be correctly parsed by JsonConvert.DeserializeObjectm, dont add anything redundant that this lib can not regconize.
                Do not include any extraneous characters, symbols, punctuation marks, or numerical order.
                ", cancellationToken: CancellationToken.None).ConfigureAwait(false);

            var optionsDto = JsonConvert.DeserializeObject<List<OptionDto>>(response);

            var options = optionsDto?.Select(x => new Option(x.GameId, x.Name)).ToList();

            return options;
        }
    }
}
