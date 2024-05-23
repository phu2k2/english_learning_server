using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace english_learning_server.Dtos.Game
{
    public class GameDto
    {
        public Guid Id { get; set; }

        public string? Kind { get; set; }

        public string? Question { get; set; }

        public string? RightAnswer { get; set; }

        public string? SoundFilePath { get; set; }

        public Guid TopicId { get; set; }
    }
}