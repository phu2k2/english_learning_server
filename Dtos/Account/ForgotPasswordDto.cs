using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Account
{
    public class ForgotPasswordDto
    {
        [Required]
        public string Email { get; set; } = null!;
    }
}