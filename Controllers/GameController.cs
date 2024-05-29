using api.Extensions;
using english_learning_server.Dtos.Game;
using english_learning_server.Interfaces;
using english_learning_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace english_learning_server.Controllers
{
    [ApiController]
    [Route("api/games")]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly IRepository<Game> _gameRepo;

        private readonly IGoogleCloudService _googleCloudService;

        public GameController(IRepository<Game> gameRepo, IGoogleCloudService googleCloudService)
        {
            _gameRepo = gameRepo;
            _googleCloudService = googleCloudService;
        }

        [HttpPost("{gameId:guid}/upload/voice")]
        public async Task<IActionResult> UploadVoiceOfUser(IFormFile audioFile, [FromRoute] Guid gameId)
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            var userId = User.GetUsername();
            var fileName = $"voice__{gameId}__{userId}.wav";

            using var memoryStream = new MemoryStream();
            await audioFile.CopyToAsync(memoryStream);

            var mediaLink = await _googleCloudService.UploadFileToBucket(fileName, memoryStream,"audio/wav");

            return Ok(new { MediaLink = mediaLink });
        }

        [HttpPost("voice/translate")]
        public async Task<IActionResult> TranslateVoiceOfUser([FromBody] TranslateVoiceDto translateVoiceDto)
        {
            string transcribedText = await _googleCloudService.TransSpeechToText(translateVoiceDto.WavFileGcsUri);

            return Ok(
                new 
                {
                    success = true,
                    text = transcribedText
                }
            );
        }
    }
}
