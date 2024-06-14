using System.ComponentModel.DataAnnotations;

namespace english_learning_server.Dtos.Option
{
    public class OptionDto
    {
        public Guid Id { get; set; }
        public Guid GameId {get; set;}
        public string Name { get; set; } = null!;
        public string? PhotoFilePath { get; set; }
        public int? BlankPosition { get; set; }
    }
}
