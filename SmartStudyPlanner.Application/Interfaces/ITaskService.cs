using Task = SmartStudyPlanner.Domain.Entities.Task;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface ITaskService
    {
        Task? GetTaskById(int taskId);
        IEnumerable<Task> GetAllTasks();
        IEnumerable<Task> GetAllByUserId(int userId);
        IEnumerable<Task> GetTasksForToday(int userId);
        IEnumerable<Task> GetTasksCompletedByUserId(int userId);
        Task AddTask(Task task);
        void UpdateTask(Task task);
        void ChangeTaskStatus(int taskId);
        void DeleteTask(int taskId);
    }
}
