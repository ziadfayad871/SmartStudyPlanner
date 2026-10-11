using System.ComponentModel.DataAnnotations;

namespace SmartStudyPlanner.Application.DTOs
{
    public class SubjectDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Subject name is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500,
            ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Difficulty must be between 0 and 100.")]
        public int Difficulty { get; set; }

        [Range(0, 100, ErrorMessage = "Progress must be between 0 and 100.")]
        public int Progress { get; set; }

        public DateTime? ExamDate { get; set; }

        [RegularExpression(@"^#([0-9A-Fa-f]{6})$",
            ErrorMessage = "Color must be a valid hex color (e.g. #F97316).")]
        public string Color { get; set; } = "#F97316";

        // Assigned server-side; not validated from the form.
        public int UserId { get; set; }
    }
}