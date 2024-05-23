using english_learning_server.Dtos.Account;
using english_learning_server.Interfaces;
using english_learning_server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace english_learning_server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<User> _userManager;

        private readonly ITokenService _tokenService;

        private readonly SignInManager<User> _signInManager;

        private readonly IProfileRepository _profileRepo;

        public AccountController(ILogger<AccountController> logger, UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager, IProfileRepository profileRepo)
        {   
            _logger = logger;
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _profileRepo = profileRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

                if (user == null) return Unauthorized("Invalid userName");

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (!result.Succeeded)
                {
                    return Unauthorized("User not found or password is incorrect");
                }
                else
                {
                    return Ok(
                        new NewUserDto
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            Token = _tokenService.CreateToken(user)
                        }
                    );
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                };

                var profile = new Profile
                {
                    UserId = user.Id,
                };

                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

                var createdProfile = await _profileRepo.CreateProfileAsync(profile);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}