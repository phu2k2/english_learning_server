using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Account.Commamd
{
    public class ResetPasswordCommandDto
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Otp { get; set; } = null!;
        [Required]
        public string NewPassword { get; set; } = null!;
    }
}
