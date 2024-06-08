using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Account.Commamd
{
    public class RegisterCommandDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
