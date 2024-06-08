﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace english_learning_server.Models;

[Table("profile_game")]
[Index("GameId", Name = "GameId")]
[Index("ProfileId", Name = "ProfileId")]
public partial class ProfileGame
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProfileId { get; set; }

    public Guid GameId { get; set; }

    public bool? IsPlayed { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("ProfileGames")]
    public virtual Game Game { get; set; } = null!;

    [ForeignKey("ProfileId")]
    [InverseProperty("ProfileGames")]
    public virtual Profile Profile { get; set; } = null!;
}
