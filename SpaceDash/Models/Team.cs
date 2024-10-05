using System.ComponentModel.DataAnnotations;

namespace SpaceDash.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string GamePin { get; set; }
        public List<Player> Players { get; set; }
    }

    public class Player
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int Score { get; set; }
        public int TimeReward { get; set; }
    }

    public class Device
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Location { get; set; }
    }

    public class Challenge
    {
        public int Id { get; set; }
        [Required]
        public string Type { get; set; } // Trivia, HighFive, Sudoku, etc.
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public string Category { get; set; }
        public List<Answer> Answers { get; set; }
        public int CorrectAnswerId { get; set; }
    }

    

    
}

