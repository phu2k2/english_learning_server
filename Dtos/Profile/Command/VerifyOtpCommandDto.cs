using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Profile.Command
{
    public class VerifyOtpCommandDto
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Otp { get; set; } = null!;
    }
}
