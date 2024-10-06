using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpaceDash.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required] // Ensure this is always populated
        public string GamePin { get; set; }

        public List<Player> Players { get; set; }
    }
}
