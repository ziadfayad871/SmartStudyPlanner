using System;

namespace SmartStudyPlanner.Application.DTOs
{
    public class UpcomingExamDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public DateTime ExamDate { get; set; }
        public int DaysLeft { get; set; }
        public int Difficulty { get; set; }
    }
}
