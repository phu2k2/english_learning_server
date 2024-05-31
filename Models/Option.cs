using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace english_learning_server.Models;

[Table("options")]
[Index("GameId", Name = "GameId")]
public partial class Option
{
    [Key]
    public Guid Id { get; set; }

    public Guid GameId { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [ForeignKey("GameId")]
    [InverseProperty("Options")]
    public virtual Game Game { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Option"/> class
    /// </summary>
    public Option(Guid gameId, string name)
    {
        GameId = gameId;
        Name = name;
    }
}
