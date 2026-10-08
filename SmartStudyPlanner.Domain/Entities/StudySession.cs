using SmartStudyPlanner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Domain.Entities
{
    public class StudySession
    {
        /*
       1-   Id
2 -   SubjectId
3 -   Date
4 -   StartTime
5 -   EndTime
6 -   Duration
7 -   Status
        */
        public int Id { get; set; }
        // navigation property
        public Subject Subject { get; set; } = null!;
        // not null for SubjectId
        public int SubjectId { get; set; } 
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public SessionStatus Status { get; set; }
    }
}
