using english_learning_server.Interfaces;
using english_learning_server.Mappers;
using english_learning_server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace english_learning_server.Controllers
{
    [ApiController]
    [Route("api/topics")]
    public class TopicController : ControllerBase
    {
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<Game> _gameRepo;

        public TopicController(IRepository<Topic> topicRepo, IRepository<Game> gameRepo)
        {
            _gameRepo = gameRepo;
            _topicRepo = topicRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopic()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var topics = _topicRepo.GetAll().ToList();

                if (topics is null)
                {
                    return NotFound("Topics not found");
                }

                var topicDto = topics.Select(t => t.ToTopicDto()).ToList();

                return Ok(topicDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{topicId:guid}/games")]
        [Authorize]
        public async Task<IActionResult> GetGamesByTopic([FromRoute] Guid topicId, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var games = await _gameRepo.GetWhereAsync(x => x.TopicId == topicId, cancellationToken).ConfigureAwait(false);

                var gamesDto = games.Select(x => x.ToGameDto(topicId)).ToList();

                return Ok(gamesDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
