namespace SpaceDash.Models
{
    public class GameSession
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; }
        public int CurrentChallengeId { get; set; }
        public Challenge CurrentChallenge { get; set; }
        public bool IsCompleted { get; set; }
        public int Score { get; set; }
        public int TimeReward { get; set; }

    }
}
