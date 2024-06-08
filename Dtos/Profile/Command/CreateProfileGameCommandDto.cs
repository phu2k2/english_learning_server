using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace english_learning_server.Dtos.Profile.Command
{
    public class CreateProfileGameCommandDto
    {
        [Required]
        public Guid ProfileId { get; set; }
        [Required]
        public Guid GameId { get; set; }
        [Required]
        public bool IsPlayed { get; set; }
    }
}