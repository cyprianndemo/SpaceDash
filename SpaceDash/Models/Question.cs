using System.ComponentModel.DataAnnotations;

namespace SpaceDash.Models
{
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
