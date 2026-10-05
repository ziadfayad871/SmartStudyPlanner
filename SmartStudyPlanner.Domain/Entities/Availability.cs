using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Domain.Entities
{
    public class Availability
    {
        /* Id
UserId
DayOfWeek
AvailableFrom
AvailableTo*/
        public int Id { get; set; }
        public int UserId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan AvailableFrom { get; set; }
        public TimeSpan AvailableTo { get; set; }
        // Navigation property to the User entity
        public User User { get; set; } = null!;
    }
}
