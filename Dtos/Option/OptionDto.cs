using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Option
{
    public class OptionDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid gameId {get; set;}
        [Required]
        public string Name { get; set; }
    }
}