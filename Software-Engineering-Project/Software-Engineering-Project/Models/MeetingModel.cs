using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class MeetingModel
    {
        public string Professor { get; set; }
        public string Student { get; set; }
        public string DateTime { get; set; }

        public string Type { get; set; }

    }
}
