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
    }
}
