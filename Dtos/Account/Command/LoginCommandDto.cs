using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Account.Commamd
{
    public class LoginQueryDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        [Required]
        public string Password { get; set; } = null!;
    }
}
