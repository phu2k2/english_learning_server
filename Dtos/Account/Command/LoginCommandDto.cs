using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Account.Commamd
{
    public class LoginQueryDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}

