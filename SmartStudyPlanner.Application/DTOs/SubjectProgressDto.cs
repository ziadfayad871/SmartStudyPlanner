namespace SmartStudyPlanner.Application.DTOs
{
    public class SubjectProgressDto
    {
        public string SubjectName { get; set; } = string.Empty;
        // progress percentage of the subject, rounded to 2 decimal places
        public double ProgressPercentage { get; set; }
        // color of the subject, in hex format (e.g. #FF5733)
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^#([A-Fa-f0-9]{6})$", ErrorMessage = "Color must be in hex format (e.g. #FF5733)")]
        public string Color { get; set; } = string.Empty;
    }
}