using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Account.Commamd
{
    public class ForgotPasswordCommandDto
    {
        [Required]
        public string Email { get; set; } = null!;
    }
}