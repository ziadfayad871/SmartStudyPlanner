using System;
using SmartStudyPlanner.Domain.Enums;

namespace SmartStudyPlanner.Application.DTOs
{
    public class TodayPlan
    {
        public int SessionId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public SessionStatus Status { get; set; }
    }
}
