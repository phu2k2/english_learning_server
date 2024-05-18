using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace english_learning_server.Dtos.Profile
{
    public class ProfileDto
    {
        public Guid Id { get; set; }

        public bool? Sex { get; set; }

        public DateTime? Birthday { get; set; }

        public bool? Status { get; set; }
    }
}