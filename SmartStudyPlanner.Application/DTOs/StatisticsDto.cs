using System;
using System.Collections.Generic;

namespace SmartStudyPlanner.Application.DTOs
{
    public class StatisticsDto
    {
        public int TotalStudyMinutes { get; set; }
        public int SessionsCompleted { get; set; }
        public int TasksCompleted { get; set; }
        public double AverageProgress { get; set; }
        public List<SubjectProgressDto> SubjectProgress { get; set; } = new List<SubjectProgressDto>();
        public List<StudyMinutesByDayDto> StudyMinutesByDay { get; set; } = new List<StudyMinutesByDayDto>();
        public List<StudyMinutesBySubjectDto> StudyMinutesBySubject { get; set; } = new List<StudyMinutesBySubjectDto>();
    }
}
