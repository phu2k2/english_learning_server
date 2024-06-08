using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Profile.Command
{
    public class UpdateProfileCommandDto
    {
        public bool? Sex { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
        public bool? Status { get; set; }
    }
}
