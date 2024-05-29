using english_learning_server.Interfaces;
using english_learning_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace english_learning_server.Controllers
{
    [ApiController]
    [Route("api/generation")]
    [Authorize]
    public class DataGenerationController : ControllerBase
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
        /// Generate sentence by topic
        /// </summary>
        [HttpGet("sentences/{topicId:guid}")]
        public async Task<IActionResult> generateSentenceByTopic([FromRoute] Guid topicId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var topic = await _topicRepo.GetFirstOrDefaultAsync(x => x.Id == topicId);

                if (topic == null)
                {
                    return NotFound();
                }

                var response = await _langChainService.GenerateSentenceByTopic(topic.Name);

                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}