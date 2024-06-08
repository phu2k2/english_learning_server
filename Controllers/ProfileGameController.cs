using api.Extensions;
using english_learning_server.Dtos.Common;
using english_learning_server.Dtos.Profile.Command;
using english_learning_server.Failure;
using english_learning_server.Interfaces;
using english_learning_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace english_learning_server.Controllers
{
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiErrorResponse))]
    [Route("api/profileGames")]
    [Authorize]
    public class ProfileGameController : ApiControllerBase
    {
        private readonly IRepository<ProfileGame> _profileGameRepo;

        public ProfileGameController(IRepository<ProfileGame> profileGameRepo)
        {
            _profileGameRepo = profileGameRepo;
        }

        /// <summary>
        /// Create profile game
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> CreateProfileGame([FromBody] CreateProfileGameCommandDto createProfileGameCommandDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var profileGame = new ProfileGame(createProfileGameCommandDto.ProfileId, createProfileGameCommandDto.GameId, createProfileGameCommandDto.IsPlayed);

            await _profileGameRepo.AddAsync(profileGame);
            await _profileGameRepo.SaveChangesAsync();

            return Ok(new ApiResponse { Message = "Profile game created successfully" });
        }
    }
}
