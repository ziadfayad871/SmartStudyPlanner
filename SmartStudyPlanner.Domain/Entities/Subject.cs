using SmartStudyPlanner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Domain.Entities
{
    public class Subject
    { 
        public int Id { get; set; }
        // not null for name
        public string Name { get; set; } = string.Empty; 
        // not null for description
        public string Description { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; }
        // not null for difficulty

        public int Progress { get; set; }
        // not null for exam date
        public DateTime? ExamDate { get; set; } 
        public ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
