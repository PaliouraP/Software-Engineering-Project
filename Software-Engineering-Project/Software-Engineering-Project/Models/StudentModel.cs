using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class StudentModel : UserModel
    {
        [Required]
        [Range(1930, 2022, ErrorMessage = "You have entered an invalid year.")]
        public int StartYear { get; set; }

        public string? Professor { get; set; }

        
    }
}
