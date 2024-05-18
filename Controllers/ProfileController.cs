using api.Extensions;
using english_learning_server.Interfaces;
using english_learning_server.Mappers;
using english_learning_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace english_learning_server.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepo;
        public ProfileController(IProfileRepository profileRepo, UserManager<User> userManager)
        {
            _profileRepo = profileRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProfile()
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

                var profile = await _profileRepo.GetProfileByUserIdAsync(userId);

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
    }
}