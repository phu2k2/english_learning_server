using api.Extensions;
using english_learning_server.Dtos.Profile;
using english_learning_server.Interfaces;
using english_learning_server.Mappers;
using english_learning_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace english_learning_server.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IRepository<Profile> _profileRepo;

        private readonly IRepository<ProfileGame> _profileGameRepo;

        public ProfileController(IRepository<Profile> profileRepo, IRepository<ProfileGame> profileGameRepo)
        {
            _profileRepo = profileRepo;
            _profileGameRepo = profileGameRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = User.GetUsername();

                if (userId == null)
                {
                    return Unauthorized();
                }

                var profile = await _profileRepo.GetFirstOrDefaultAsync(x => x.UserId == userId, cancellationToken).ConfigureAwait(false);

                if (profile == null)
                {
                    return NotFound();
                }

                return Ok(profile.ToProfileDto());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{profileId:guid}")]
        public async Task<IActionResult> UpdateProfile([FromRoute] Guid profileId, [FromBody] UpdateProfileDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingProfile = await _profileRepo.GetFirstOrDefaultAsync(x => x.Id == profileId, cancellationToken).ConfigureAwait(false);

                if (existingProfile is null)
                {
                    return NotFound();
                }

                existingProfile.Sex = updateDto.Sex;
                existingProfile.Birthday = updateDto.Birthday;
                existingProfile.Status = updateDto.Status;

                _profileRepo.Update(existingProfile);

                await _profileRepo.SaveChangesAsync().ConfigureAwait(false);

                return Ok(
                    new
                    {
                        success = true,
                        message = "Profile updated successfully",
                    }
                );
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch("{profileId:guid}/games/{gameId:guid}/play")]
        public async Task<IActionResult> UpdateIsPlayOfGame([FromRoute] Guid profileId, [FromRoute] Guid gameId, [FromBody] UpdateProfileGameDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var profileGame = await _profileGameRepo.GetFirstOrDefaultAsync(x => x.ProfileId == profileId && x.GameId == gameId, cancellationToken).ConfigureAwait(false);

                if (profileGame is null)
                {
                    return NotFound();
                }

                profileGame.IsPlayed = updateDto.IsPlayed;

                _profileGameRepo.UpdateOwnProperties(profileGame);

                await _profileGameRepo.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Ok(
                    new
                    {
                        success = true,
                        message = "Update game status successfully!"
                    }
                );
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}