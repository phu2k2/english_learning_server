using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Profile.Command
{
    public class UpdateImageCommandDto
    {
        [Required]
        public IFormFile ImageFile { get; set; } = null!;
    }
}
