using api.Extensions;
using english_learning_server.Dtos.Common;
using english_learning_server.Failure;
using english_learning_server.Interfaces;
using english_learning_server.Mappers;
using english_learning_server.Models;
using Microsoft.AspNetCore.Mvc;

namespace english_learning_server.Controllers
{
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiErrorResponse))]
    [Route("api/generation")]
    public class DataGenerationController : ApiControllerBase
    {
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<Game> _gameRepo;
        private readonly IRepository<Option> _optionRepo;
        private readonly ILangChainService _langChainService;
        public DataGenerationController(IRepository<Topic> topicRepo, IRepository<Game> gameRepo, IRepository<Option> optionRepo, ILangChainService langChainService)
        {
            _topicRepo = topicRepo;
            _gameRepo = gameRepo;
            _optionRepo = optionRepo;
            _langChainService = langChainService;
        }

        /// <summary>
        /// Generate games by topic
        /// </summary>
        [HttpPost("topics/{topicId:guid}/games")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> createGamesByTopic([FromRoute] Guid topicId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var topic = await _topicRepo.GetFirstOrDefaultAsync(x => x.Id == topicId);

                if (topic == null)
                {
                    return NotFoundResponse("Topic not found");
                }

                var basePictureChoiceGames = await _langChainService.FetchGamesForPictureChoiceByTopic(topicId, topic.Name).ConfigureAwait(false);

                var baseAnotherGames = await _langChainService.FetchGamesByTopic(topicId, topic.Name).ConfigureAwait(false);
                
                await _gameRepo.AddRangeAsync(basePictureChoiceGames);
                await _gameRepo.AddRangeAsync(baseAnotherGames);

                await _gameRepo.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Ok( new ApiResponse { Message = "Games created successfully" });
            }
            catch (Exception e)
            {
                return InternalServerErrorResponse(e.Message);
            }
        }

        /// <summary>
        /// Generate options for sentence scramble games by topic
        /// </summary>
        [HttpPost("topics/{topicId:guid}/options/scrambleGame")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> createOptionsForSentenceScrambleByTopic([FromRoute] Guid topicId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var games = _gameRepo.GetBy(x => x.TopicId == topicId && x.Kind == "Sentence Scramble").ToList();

                var topic = await _topicRepo.GetFirstOrDefaultAsync(x => x.Id == topicId);

                if (games == null || topic == null)
                {
                    return NotFound();
                }

                var baseOptions = await _langChainService.FetchOptionForSentenceScrambleGamesByTopic(games, topic.Name).ConfigureAwait(false);

                await _optionRepo.AddRangeAsync(baseOptions);

                await _optionRepo.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Ok( new ApiResponse { Message = "Options created successfully" });
            }
            catch (Exception e)
            {
                return InternalServerErrorResponse(e.Message);
            }
        }


        /// <summary>
        /// Generate options for picture choice games by topic
        /// </summary>
        [HttpPost("topics/{topicId:guid}/options/pictureChoice")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> createOptionsForPictureChoiceByTopic([FromRoute] Guid topicId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var games = _gameRepo.GetBy(x => x.TopicId == topicId && x.Kind == "Picture Choice").ToList();

                var topic = await _topicRepo.GetFirstOrDefaultAsync(x => x.Id == topicId);

                if (games == null || topic == null)
                {
                    return NotFoundResponse("Games or Topics not found");
                }

                var baseOptions = await _langChainService.FetchOptionForPictureChoiceByTopic(games, topic.Name).ConfigureAwait(false);

                await _optionRepo.AddRangeAsync(baseOptions);

                await _optionRepo.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Ok( new ApiResponse { Message = "Options created successfully" });
            }
            catch (Exception e)
            {
                return InternalServerErrorResponse(e.Message);
            }
        }

        /// <summary>
        /// Generate options for sentence choice games by topic
        /// </summary>
        [HttpPost("topics/{topicId:guid}/options/sentenceChoice")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> createOptionsForSentenceChoiceByTopic([FromRoute] Guid topicId, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var games = _gameRepo.GetBy(x => x.TopicId == topicId && x.Kind == "Sentence Choice").ToList();

                var topic = await _topicRepo.GetFirstOrDefaultAsync(x => x.Id == topicId);

                if (games == null || topic == null)
                {
                    return NotFoundResponse("Games or Topics not found");
                }

                var baseOptions = await _langChainService.FetchOptionForSentenceChoiceByTopic(games, topic.Name).ConfigureAwait(false);

                await _optionRepo.AddRangeAsync(baseOptions);

                await _optionRepo.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Ok( new ApiResponse { Message = "Options created successfully" });
            }
            catch (Exception e)
            {
                return InternalServerErrorResponse(e.Message);
            }
        }
    }
}
