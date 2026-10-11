using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Infrastructure.Data;
using Task = SmartStudyPlanner.Domain.Entities.Task;

namespace SmartStudyPlanner.Infrastructure.Repositories
{
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Task> GetByUserId(int userId)
        {
            return _context.Set<Task>()
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.DueDate)
                .ToList();
        }

        public IEnumerable<Task> GetBySubjectId(int subjectId)
        {
            return _context.Set<Task>()
                .Where(t => t.SubjectId == subjectId)
                .OrderBy(t => t.DueDate)
                .ToList();
        }

        public IEnumerable<Task> GetByUserIdAndDate(int userId, DateTime date)
        {
            return _context.Set<Task>()
                .Where(t => t.UserId == userId && t.DueDate.Date == date.Date)
                .OrderBy(t => t.DueDate)
                .ToList();
        }
    }
}
