using SmartStudyPlanner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Range(0, 100, ErrorMessage = "Progress must be between 0 and 100")]
        public int Progress { get; set; }
        // not null for exam date
        public DateTime? ExamDate { get; set; } 
        public ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public int  EstimatedMinutes { get; set; } 
        // hex color code for the subject card (e.g. #F97316)
        public string Color { get; set; } = "#F97316";
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
