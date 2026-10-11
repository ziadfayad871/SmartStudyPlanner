using SmartStudyPlanner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public TimeSpan Duration { get; set; } 
        public SessionStatus Status { get; set; }
    }
}
