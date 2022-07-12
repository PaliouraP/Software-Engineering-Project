using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class MeetingModel
    {
        public string Professor { get; set; }
        public string Student { get; set; }
        public DateTime DateTime { get; set; }
        public string Duration { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

    }
}
