using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class LoginModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public bool IsLoginConfirmed { get; set; } = true;
    }
}
