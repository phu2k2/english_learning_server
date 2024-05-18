using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace english_learning_server.Models;

[Table("topics")]
public partial class Topic
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string Image { get; set; } = null!;

    public int NumberOfGame { get; set; }

    [InverseProperty("Topic")]
    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
