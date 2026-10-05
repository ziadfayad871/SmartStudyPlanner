using SmartStudyPlanner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int EstimatedMinutes { get; set; } 
        // periority : 
        public Priority Priority { get; set; }
        // IsCompleted
        public bool IsCompleted { get; set; } = false;
        // navegation property
        public Subject Subject { get; set; } = null!;
    }
}
