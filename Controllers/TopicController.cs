using api.Extensions;
using english_learning_server.Dtos.Game;
using english_learning_server.Dtos.Topic;
using english_learning_server.Dtos.Topic.Response;
using english_learning_server.Failure;
using english_learning_server.Interfaces;
using english_learning_server.Mappers;
using english_learning_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace english_learning_server.Controllers
{
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiErrorResponse))]
    [Route("api/topics")]
    [Authorize]
    public class TopicController : ApiControllerBase
    {
        private readonly IRepository<Topic> _topicRepo;
        private readonly IRepository<Game> _gameRepo;

        private readonly IRepository<Profile> _profileRepo;

        public TopicController(IRepository<Topic> topicRepo, IRepository<Game> gameRepo, IRepository<Profile> profileRepo)
        {
            _gameRepo = gameRepo;
            _topicRepo = topicRepo;
            _profileRepo = profileRepo;
        }

        /// <summary>
        /// Get list of topics
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTopicsResponseDto))]
        public async Task<IActionResult> GetAllTopic()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var topics = _topicRepo.GetAll().Include(t => t.Games).ThenInclude(g => g.ProfileGames).ToList();

                if (topics is null)
                {
                    return NotFoundResponse("Topics not found");
                }

                var userId = User.GetUsername();

                var profileId = _profileRepo.GetFirstOrDefault(x => x.UserId == userId);

                var topicsDto = topics.ToTopicsResponseDto(profileId.Id);

                return Ok(topicsDto);
            }
            catch (Exception e)
            {
                return InternalServerErrorResponse(e.Message);
            }
        }
    }
}
