using System.ComponentModel.DataAnnotations;

namespace Software_Engineering_Project.Models
{
    public class UserModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must contain at least 6 characters.")]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"male|female|other", ErrorMessage = "Please enter 'male' , 'female' or 'other'.")]
        public string Gender { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        public string Phone { get; set; }

        public string? Role { get; set; } = "student";
    }
}
