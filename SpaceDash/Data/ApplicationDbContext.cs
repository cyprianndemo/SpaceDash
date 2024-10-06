using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace SpaceDash.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameSession>()
                .HasOne(gs => gs.CurrentChallenge)
                .WithMany()
                .HasForeignKey(gs => gs.CurrentChallengeId)
                .IsRequired(false) // Make the relationship optional
                .OnDelete(DeleteBehavior.SetNull); // Set CurrentChallengeId to null if the Challenge is deleted

            modelBuilder.Entity<Challenge>()
                .HasOne(c => c.GameSession)
                .WithMany(gs => gs.Challenges)
                .HasForeignKey(c => c.GameSessionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}