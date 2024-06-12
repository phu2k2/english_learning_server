using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace english_learning_server.Models;

[Table("profiles")]
[Index("UserId", Name = "UserId", IsUnique = true)]
public partial class Profile
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public bool? Sex { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Birthday { get; set; }

    public bool? Status { get; set; }

    public string? AvatarFilePath { get; set; }

    public string UserId { get; set; } = null!;

    [InverseProperty("Follower")]
    public virtual ICollection<Follow> FollowFollowers { get; set; } = new List<Follow>();

    [InverseProperty("Profile")]
    public virtual ICollection<Follow> FollowProfiles { get; set; } = new List<Follow>();

    [InverseProperty("Profile")]
    public virtual ICollection<ProfileGame> ProfileGames { get; set; } = new List<ProfileGame>();

    [ForeignKey("UserId")]
    [InverseProperty("Profile")]
    public virtual User User { get; set; } = null!;
}
