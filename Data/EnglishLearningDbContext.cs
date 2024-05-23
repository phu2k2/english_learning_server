using english_learning_server.Interfaces;
using english_learning_server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace english_learning_server.Data;

public partial class EnglishLearningDbContext : IdentityDbContext<User>
{
    public EnglishLearningDbContext()
    {
    }

    public EnglishLearningDbContext(DbContextOptions<EnglishLearningDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Follow> Follows { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<ProfileGame> ProfileGames { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Follower).WithMany(p => p.FollowFollowers).HasConstraintName("follows_ibfk_2").OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Profile).WithMany(p => p.FollowProfiles).HasConstraintName("follows_ibfk_1").OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.HasData(new IdentityRole { Name = "User", NormalizedName = "USER" });
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Topic).WithMany(p => p.Games).HasConstraintName("games_ibfk_1").OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Game).WithMany(p => p.Options).HasConstraintName("options_ibfk_1").OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.User).WithOne(p => p.Profile).HasConstraintName("profiles_ibfk_1").OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProfileGame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Game).WithMany(p => p.ProfileGames).HasConstraintName("profile_game_ibfk_2").OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Profile).WithMany(p => p.ProfileGames).HasConstraintName("profile_game_ibfk_1").OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        base.OnModelCreating(modelBuilder);
    }
}
