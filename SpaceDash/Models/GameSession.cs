﻿namespace SpaceDash.Models
{
    public class GameSession
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int Score { get; set; }
        public string? TimeTaken { get; set; }
        public string? SessionId { get; set; }
        public int TimeReward { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
        public int? CurrentChallengeId { get; set; }
        public Challenge CurrentChallenge { get; set; }
        public List<Challenge> Challenges { get; set; }
    }
}