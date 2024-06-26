using api.Extensions;
using english_learning_server.Dtos.Account.Commamd;
using english_learning_server.Dtos.Account.Response;
using english_learning_server.Dtos.Common;
using english_learning_server.Dtos.Profile.Command;
using english_learning_server.Failure;
using english_learning_server.Interfaces;
using english_learning_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace english_learning_server.Controllers
{
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiErrorResponse))]
    [Route("api/users")]
    public class AccountController : ApiControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly SignInManager<User> _signInManager;
        private readonly IProfileRepository _profileRepo;
        private readonly IGoogleCloudService _googleCloudService;
        private static readonly Dictionary<string, string> _otpStore = new Dictionary<string, string>();

        public AccountController(UserManager<User> userManager, ITokenService tokenService, IEmailService emailService, SignInManager<User> signInManager, IProfileRepository profileRepo, IGoogleCloudService googleCloudService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _signInManager = signInManager;
            _profileRepo = profileRepo;
            _googleCloudService = googleCloudService;
        }

        /// <summary>
        /// Login user
        /// </summary>
        [HttpPost("signIn")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewUserDto))]
        public async Task<IActionResult> Login([FromBody] LoginQueryDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (loginDto.UserName == null && loginDto.Email == null)
                {
                    return BadRequestResponse("Username or Email is required");
                }

                var user = loginDto.Email != null
                            ? await _userManager.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email)
                            : await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

                if (user == null) return UnauthorizedResponse("Invalid userName");

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (!result.Succeeded)
                {
                    return UnauthorizedResponse("User not found or password is incorrect");
                }
                else
                {
                    return Ok(
                        new NewUserDto
                        {
                            UserName = user.UserName!,
                            Email = user.Email!,
                            Token = _tokenService.CreateToken(user)
                        }
                    );
                }
            }
            catch (Exception e)
            {
                return InternalServerErrorResponse(e.Message);
            }
        }

        /// <summary>
        /// Register user
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewUserDto))]
        public async Task<IActionResult> Register([FromBody] RegisterCommandDto registerDto)
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


                if (createdUser.Succeeded)
                {
                    var createdProfile = await _profileRepo.CreateProfileAsync(profile);

                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = user.UserName!,
                                Email = user.Email!,
                                Token = _tokenService.CreateToken(user)
                            }
                        );
                    }
                    else
                    {
                        return InternalServerErrorResponse(roleResult.Errors);
                    }
                }
                else
                {
                    return InternalServerErrorResponse(createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return InternalServerErrorResponse(e.Message);
            }
        }

        /// <summary>
        /// Sign out user
        /// </summary>
        [HttpPost("signOut")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return UnauthorizedResponse("No user is currently logged in");
            }

            await _signInManager.SignOutAsync();

            return Ok(new ApiResponse { Message = "User logged out successfully" });
        }

        /// <summary>
        /// Forgot password
        /// </summary>
        [HttpPost("forgotPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommandDto forgotPasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

                if (user == null)
                {
                    return NotFoundResponse("User not found");
                }

                var otp = _emailService.GenerateOTP();

                _otpStore[user.Email!] = otp;

                var htmlMessage = await _googleCloudService.EmailHtmlMessage(otp, DateTime.Now.ToString("dd MMM, yyyy"), user.UserName!);

                await _emailService.SendEmailAsync(user.Email!, "Reset Password", htmlMessage);

                return Ok(new ApiResponse { Message = "OTP has been sent to your email successfully" });
            }
            catch (Exception e)
            {
                return InternalServerErrorResponse(e.Message);
            }
        }

        /// <summary>
        /// Verify OTP
        /// </summary>
        [HttpPost("verifyOtp")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpCommandDto verifyOtpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_otpStore.TryGetValue(verifyOtpDto.Email, out var otp) || otp != verifyOtpDto.Otp)
            {
                return UnauthorizedResponse("Invalid OTP");
            }

            return Ok(new ApiResponse { Message = "OTP verified successfully" });
        }

        /// <summary>
        /// Reset password
        /// </summary>
        [HttpPost("resetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommandDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user == null)
            {
                return NotFoundResponse("User not found");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordDto.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new ApiResponse { Message = "Password reset successfully" });
            }
            else
            {
                return InternalServerErrorResponse(result.Errors);
            }
        }
    }
}
