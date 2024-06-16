using api.Extensions;
using english_learning_server.Dtos.Game.Command;
using english_learning_server.Dtos.Game.Response;
using english_learning_server.Dtos.ProfileGame.Query;
using english_learning_server.Dtos.ProfileGame.Response;
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
    [Route("api/games")]
    [Authorize]
    public class GameController : ApiControllerBase
    {
        private readonly IRepository<Game> _gameRepo;
        private readonly ILevenshteinDistanceService _levenshteinDistanceService;
        private readonly IGoogleCloudService _googleCloudService;

        public GameController(IGoogleCloudService googleCloudService,ILevenshteinDistanceService levenshteinDistanceService, IRepository<Game> gameRepo)
        {
            _googleCloudService = googleCloudService;
            _levenshteinDistanceService = levenshteinDistanceService;
            _gameRepo = gameRepo;
        }

        /// <summary>
        /// Upload voice of user for game
        /// </summary>
        [HttpPost("{gameId:guid}/upload/voice")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadVoiceOfUserResponse))]
        public async Task<IActionResult> UploadVoiceOfUser([FromForm] UploadVoiceOfUserCommandDto file, [FromRoute] Guid gameId)
        {
            if (file.audioFile.Length == 0)
            {
                return BadRequestResponse("File is empty");
            }

            var userId = User.GetUsername();
            var fileName = $"voice__{gameId}__{userId}.wav";

            using var memoryStream = new MemoryStream();
            await file.audioFile.CopyToAsync(memoryStream);

            var mediaLink = await _googleCloudService.UploadFileToBucket(fileName, memoryStream, "audio/wav");

            return Ok(new UploadVoiceOfUserResponse { MediaLink = mediaLink });
        }

        /// <summary>
        /// Evaluate the similarity of voice of user for game
        /// </summary>
        [HttpPost("voice/similarity")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslateVoiceResponseDto))]
        public async Task<IActionResult> TranslateVoiceOfUser([FromBody] TranslateVoiceCommandDto translateVoiceDto)
        {
            var transcribedText = await _googleCloudService.TransSpeechToText(translateVoiceDto.WavFileGcsUri);

            if (transcribedText == null)
            {
                return BadRequestResponse("Error in transcribing voice");
            }   

            var normalizedLevenshteinDistance = _levenshteinDistanceService.CalculateNormalizedLevenshteinDistance(transcribedText, translateVoiceDto.RightAnswer);

            bool similarity = normalizedLevenshteinDistance >= 0.8;

            return Ok(new TranslateVoiceResponseDto { Text = transcribedText, Similarity = similarity});
        }

        /// <summary>
        /// Get list of games by topic and profile
        /// </summary>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProfileGamesResponseDto))]
        public async Task<IActionResult> GetGamesByTopicAndProfile([FromQuery] GetProfileGamesQueryDto profileGamesResponseDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var games = await _gameRepo.GetBy(g => g.TopicId == profileGamesResponseDto.TopicId).Include(g => g.ProfileGames).Include(g => g.Options).ToListAsync();

                if (games is null)
                {
                    return NotFoundResponse("Games not found");
                }

                var gamesDto = games.ToProfileGamesResponseDto(profileGamesResponseDto);

                return Ok(gamesDto);
            }
            catch (Exception e)
            {
                return InternalServerErrorResponse(e.Message);
            }
        }


        /// <summary>
        /// Get list of games by topic and profile
        /// </summary>
        [HttpPost("synthesizeSpeech")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(File))]
        public async Task<IActionResult> SynthesizeSpeech([FromQuery] SynthesizeSpeechCommandDto synthesizeSpeechQueryDto)
        {
            var response = await _googleCloudService.SynthesizeSpeech(synthesizeSpeechQueryDto.Text);

            return Ok(File(response, "audio/mpeg"));
        }
    }
}
