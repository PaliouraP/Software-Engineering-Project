namespace Software_Engineering_Project.Models
{
    public class ThesisModel
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Professor { get; set; }

        public string Title { get; set; }

        public DateOnly ThesisStartDate { get; set; }

        public int Grade { get; set; }

        public string Language { get; set; }

        public string Technology { get; set; }

        public IFormFile File { get; set; }

        public int version { get; set; }

    }
}
