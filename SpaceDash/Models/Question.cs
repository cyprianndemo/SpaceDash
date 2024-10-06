using System.Collections.Generic;

namespace SpaceDash.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int CorrectAnswerId { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public bool IsCompleted { get; set; } // Add this property
        public int ChallengeId { get; set; }
    }

}
