using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace english_learning_server.Dtos.Option
{
    public class CreateOptionDto
    {
        [Required]
        public Guid GameId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
