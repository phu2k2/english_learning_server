using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace english_learning_server.Models;

[Table("follows")]
[Index("FollowerId", Name = "FollowerId")]
[Index("ProfileId", Name = "ProfileId")]
public partial class Follow
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProfileId { get; set; }

    public Guid FollowerId { get; set; }

    [ForeignKey("FollowerId")]
    [InverseProperty("FollowFollowers")]
    public virtual Profile Follower { get; set; } = null!;

    [ForeignKey("ProfileId")]
    [InverseProperty("FollowProfiles")]
    public virtual Profile Profile { get; set; } = null!;
}
