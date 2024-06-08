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
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Follower).WithMany(p => p.FollowFollowers).HasForeignKey(d => d.FollowerId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("follows_ibfk_2");

            entity.HasOne(d => d.Profile).WithMany(p => p.FollowProfiles).HasForeignKey(d => d.ProfileId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("follows_ibfk_1");
        });

        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.HasData(new IdentityRole { Name = "User", NormalizedName = "USER" });
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Topic).WithMany(p => p.Games).HasForeignKey(d => d.TopicId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("games_ibfk_1");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Game).WithMany(p => p.Options).HasForeignKey(d => d.GameId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("options_ibfk_1");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.User).WithOne(p => p.Profile).HasForeignKey<Profile>(p => p.UserId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("profiles_ibfk_1");
        });

        modelBuilder.Entity<ProfileGame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Game).WithMany(p => p.ProfileGames).HasForeignKey(d => d.GameId).HasConstraintName("profile_game_ibfk_2").OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Profile).WithMany(p => p.ProfileGames).HasForeignKey(d => d.ProfileId).HasConstraintName("profile_game_ibfk_1").OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        // Explicitly set table names for Identity tables and others
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("aspnetusers");
        });

        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable("aspnetroles");
        });

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("aspnetuserroles");
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("aspnetuserclaims");
        });

        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("aspnetuserlogins");
        });

        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("aspnetroleclaims");
        });

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("aspnetusertokens");
        });
    }
}
