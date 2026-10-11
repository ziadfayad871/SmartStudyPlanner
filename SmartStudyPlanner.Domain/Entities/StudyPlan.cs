using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SmartStudyPlanner.Domain.Entities
{
    public class StudyPlan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public User User { get; set; } = null!;
        public ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();
        // navigation property for the many-to-many relationship with Subject
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; } = "A study plan for a user.";

    }
}
