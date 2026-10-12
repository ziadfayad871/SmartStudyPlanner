using SmartStudyPlanner.Domain.Entities;
using SmartStudyPlanner.Application.DTOs;

namespace SmartStudyPlanner.Web.ViewModels
{
    public class CalendarDay
    {
        public DateTime Date { get; set; }
        public bool InMonth { get; set; }
        public bool IsToday { get; set; }
        public bool IsSelected { get; set; }
        public int SessionCount { get; set; }
    }

    public class CalendarViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<List<CalendarDay>> Weeks { get; set; } = new();
        public DateTime SelectedDate { get; set; }
        public List<StudySession> SelectedSessions { get; set; } = new();
        public Dictionary<int, string> SubjectNames { get; set; } = new();
        public List<UpcomingExamDto> UpcomingExams { get; set; } = new();
    }
}
