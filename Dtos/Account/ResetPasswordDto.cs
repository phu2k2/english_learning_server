using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace english_learning_server.Dtos.Account
{
    public class ResetPasswordDto
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Token { get; set; } = null!;
        [Required]
        public string NewPassword { get; set; } = null!;
    }
}