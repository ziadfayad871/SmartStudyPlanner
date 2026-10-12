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

        public bool EmailNotifications { get; set; } = true;

        public bool StudyReminders { get; set; } = true;

        public bool ExamAlerts { get; set; } = true;

        public string Theme { get; set; } = "light";

        public bool ProfilePublic { get; set; } = true;

        public bool ShowEmail { get; set; } = false;

        public ICollection<StudyPlan> StudyPlans { get; set; } = new List<StudyPlan>();
        public ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
    }
}
