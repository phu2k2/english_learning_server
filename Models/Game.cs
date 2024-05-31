﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Api.Gax;
using Microsoft.EntityFrameworkCore;

namespace english_learning_server.Models;

[Table("games")]
[Index("TopicId", Name = "TopicId")]
public partial class Game
{
    [Key]
    public Guid Id { get; set; }

    [Column(TypeName = "enum('Picture Choice','Sentence Scramble','Sentence Choice','Echo Repeat')")]
    public string? Kind { get; set; }

    [Column(TypeName = "text")]
    public string? Question { get; set; }

    [StringLength(255)]
    public string? RightAnswer { get; set; }

    [StringLength(255)]
    public string? SoundFilePath { get; set; }

    public Guid TopicId { get; set; }

    [InverseProperty("Game")]
    public virtual ICollection<Option> Options { get; set; } = new List<Option>();

    [InverseProperty("Game")]
    public virtual ICollection<ProfileGame> ProfileGames { get; set; } = new List<ProfileGame>();

    [ForeignKey("TopicId")]
    [InverseProperty("Games")]
    public virtual Topic Topic { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/> class
    /// </summary>
    public Game(string? kind, string? question, string? rightAnswer, Guid topicId)
    {
        Kind = kind;
        Question = question;
        RightAnswer = rightAnswer;
        TopicId = topicId;
    }
}
