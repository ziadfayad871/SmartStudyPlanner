using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.DTOs
{
    public class DashboardDto
    {
        public string UserName { get; set; } = string.Empty;
        public DateTime TodayDate { get; set; }
        public int SubjectsCount { get; set; }
        public int TodaySessionsCount { get; set; }
        public int DaysUntilNextExam { get; set; }
        // TodayProgressPercent :
        public double TodayProgressPercent { get; set; }
        // StudiedMinutesToday 
        public int StudiedMinutesToday { get; set; }
        // RemainingMinutesToday
        public int RemainingMinutesToday { get; set; }
        public int CompletedTasksCount { get; set; }

        public  IEnumerable< UpcomingExamDto> UpcomingExams { get; set; } = new List<UpcomingExamDto>();
        public IEnumerable<TodayPlan> TodayPlans { get; set; } = new List<TodayPlan>();   
    }
}
