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
        private readonly IGoogleCloudService _googleCloudService;

        public ProfileController(IRepository<Profile> profileRepo, IRepository<ProfileGame> profileGameRepo, IGoogleCloudService googleCloudService)
        {
            _profileRepo = profileRepo;
            _profileGameRepo = profileGameRepo;
            _googleCloudService = googleCloudService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
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

        [HttpPut("{profileId:guid}")]
        public async Task<IActionResult> UpdateProfile([FromRoute] Guid profileId, [FromBody] UpdateProfileDto updateDto, CancellationToken cancellationToken)
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

        [HttpPatch("{profileId:guid}/games/{gameId:guid}/play")]
        public async Task<IActionResult> UpdateIsPlayOfGame([FromRoute] Guid profileId, [FromRoute] Guid gameId, [FromBody] UpdateProfileGameDto updateDto, CancellationToken cancellationToken)
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

        [HttpPost("{profileId:guid}/avatar/upload")]
        public async Task<IActionResult> UploadImageOfUser(IFormFile imageFile, [FromRoute] Guid profileId)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            var fileExtension = Path.GetExtension(imageFile.FileName);
            var fileName = $"image__{profileId}{fileExtension}";

            using var memoryStream = new MemoryStream();
            await imageFile.CopyToAsync(memoryStream);

            var mediaLink = await _googleCloudService.UploadFileToBucket(fileName, memoryStream,"image/jpeg");

            var imageUrls = _googleCloudService.GetPublicAndAuthenticatedUrl(fileName);

            return Ok(
                new
                {
                    success = true,
                    authenticatedUrl = imageUrls.AuthenticatedUrl,
                    publicUrl = imageUrls.PublicUrl
                }
            );
        }
    }
}
