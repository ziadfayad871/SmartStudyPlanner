using SmartStudyPlanner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.DTOs
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; }
        public int Progress { get; set; }
        public DateTime?  ExamDate { get; set; }
    }
}
