using System.ComponentModel.DataAnnotations;
using SmartStudyPlanner.Domain.Enums;

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

        public Difficulty Difficulty { get; set; }

        [Range(0, 100, ErrorMessage = "Progress must be between 0 and 100.")]
        public int Progress { get; set; }

        public DateTime? ExamDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a user.")]
        public int UserId { get; set; }
    }
}