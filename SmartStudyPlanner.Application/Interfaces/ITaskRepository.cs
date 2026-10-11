using Task = SmartStudyPlanner.Domain.Entities.Task;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface ITaskRepository : IRepository<Task>
    {
        IEnumerable<Task> GetByUserId(int userId);
        IEnumerable<Task> GetBySubjectId(int subjectId);
        IEnumerable<Task> GetByUserIdAndDate(int userId, DateTime date);
    }
}
