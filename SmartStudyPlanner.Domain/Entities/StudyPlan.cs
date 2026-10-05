using System;
using System.Collections.Generic;
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
    }
}
