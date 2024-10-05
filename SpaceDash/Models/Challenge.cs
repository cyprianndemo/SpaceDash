using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpaceDash.Models
{
    public class Challenge
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }
        public int Order { get; set; }


        public int DeviceId { get; set; }
        public Device Device { get; set; }

        public List<Question> Questions { get; set; }

        public int? RequiredHighFives { get; set; }
        public bool IsCompleted { get; set; } 

    }
}
