using System.Collections.Generic;

namespace SpaceDash.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int CorrectAnswerId { get; set; }
        public List<Answer> Answers { get; set; } // Assuming you have an Answer class
        public int ChallengeId { get; set; } // Assuming this links to a Challenge
        public Challenge Challenge { get; set; } // Navigation property
    }
}
