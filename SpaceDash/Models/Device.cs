using System.ComponentModel.DataAnnotations;

namespace SpaceDash.Models
{
    public class Device
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
