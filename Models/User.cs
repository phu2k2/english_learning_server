using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace english_learning_server.Models
{
    [Table("users")]
    public partial class User : IdentityUser
    {
        [InverseProperty("User")]
        public Profile? Profile { get; set; }
    }
}
