using System.ComponentModel.DataAnnotations;

namespace SpaceDash.Models
{
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

}
