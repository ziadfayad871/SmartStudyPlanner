using SmartStudyPlanner.Domain.Enums;
using TaskStatus = SmartStudyPlanner.Domain.Enums.TaskStatus;

namespace SmartStudyPlanner.Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int EstimatedMinutes { get; set; }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; } = false;
        public Subject? Subject { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
    }
}
