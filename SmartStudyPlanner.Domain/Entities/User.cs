using System.ComponentModel.DataAnnotations;

namespace SmartStudyPlanner.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string AcademicLevel { get; set; } = string.Empty;

        public string ProfilePictureUrl { get; set; } = string.Empty;

        public byte[]? ProfilePicture { get; set; }

        public string? ProfilePictureContentType { get; set; }

        public ICollection<StudyPlan> StudyPlans { get; set; } = new List<StudyPlan>();
        public ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
    }
}
