using api.Extensions;
using english_learning_server.Dtos.Common;
using english_learning_server.Dtos.Profile.Command;
using english_learning_server.Dtos.Profile.Response;
using english_learning_server.Dtos.ProfileGame.Command;
using english_learning_server.Failure;
using english_learning_server.Interfaces;
using english_learning_server.Mappers;
using english_learning_server.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace english_learning_server.Controllers
{
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiErrorResponse))]
    [Route("api/profiles")]
    [Authorize]
    public class ProfileController : ApiControllerBase
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

        /// <summary>
        /// Get profile
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProfileResponseDto))]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.GetUsername();

            var profile = await _profileRepo.GetFirstOrDefaultAsync(x => x.UserId == userId, cancellationToken).ConfigureAwait(false);

            if (profile == null)
            {
                return NotFoundResponse("Profile is not found");
            }

            return Ok(profile.ToProfileResponseDto());
        }

        /// <summary>
        /// Update profile
        /// </summary>
        [HttpPut("{profileId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> UpdateProfile([FromRoute] Guid profileId, [FromBody] UpdateProfileCommandDto updateDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProfile = await _profileRepo.GetFirstOrDefaultAsync(x => x.Id == profileId, cancellationToken).ConfigureAwait(false);

            if (existingProfile is null)
            {
                return NotFoundResponse("Profile is not found");
            }

            existingProfile.Sex = updateDto.Sex;
            existingProfile.Birthday = updateDto.Birthday;
            existingProfile.Status = updateDto.Status;

            _profileRepo.Update(existingProfile);

            await _profileRepo.SaveChangesAsync().ConfigureAwait(false);

            return Ok(new ApiResponse { Message = "Update profile successfully!" });
        }

        /// <summary>
        /// Update play status of game
        /// </summary>
        [HttpPatch("{profileId:guid}/games/{gameId:guid}/play")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> UpdatePlayStatusOfGame([FromRoute] Guid profileId, [FromRoute] Guid gameId, [FromBody] UpdateProfileGameCommandDto updateDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var profileGame = await _profileGameRepo.GetFirstOrDefaultAsync(x => x.ProfileId == profileId && x.GameId == gameId, cancellationToken).ConfigureAwait(false);

            if (profileGame is null)
            {
                return NotFoundResponse("Profile or Game is not found");
            }

            profileGame.IsPlayed = updateDto.IsPlayed;

            _profileGameRepo.UpdateOwnProperties(profileGame);

            await _profileGameRepo.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Ok(new ApiResponse { Message = "Update play status of game successfully!" });
        }

        /// <summary>
        /// Upload avatar
        /// </summary>
        [HttpPost("{profileId:guid}/uploadAvatar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadImageResponseDto))]
        public async Task<IActionResult> UploadImageOfUser([FromForm] UpdateImageCommandDto file, [FromRoute] Guid profileId)
        {
            if (file.ImageFile.Length == 0)
            {
                return BadRequestResponse("No file uploaded");
            }

            var fileExtension = Path.GetExtension(file.ImageFile.FileName);
            var fileName = $"avatar__{profileId}{fileExtension}";

            using var memoryStream = new MemoryStream();
            await file.ImageFile.CopyToAsync(memoryStream);

            var mediaLink = await _googleCloudService.UploadFileToBucket(fileName, memoryStream, "image/jpeg");

            var imageUrls = _googleCloudService.GetPublicAndAuthenticatedUrl(fileName);

            return Ok(new UploadImageResponseDto { AuthenticatedUrl = imageUrls.AuthenticatedUrl, PublicUrl = imageUrls.PublicUrl });
        }
    }
}
