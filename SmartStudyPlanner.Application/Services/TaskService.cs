using SmartStudyPlanner.Application.Interfaces;
using Task = SmartStudyPlanner.Domain.Entities.Task;

namespace SmartStudyPlanner.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task? GetTaskById(int taskId)
        {
            return _taskRepository.GetById(taskId);
        }

        public IEnumerable<Task> GetAllTasks()
        {
            return _taskRepository.GetAll()
                .OrderBy(t => t.DueDate)
                .ToList();
        }

        public IEnumerable<Task> GetAllByUserId(int userId)
        {
            return _taskRepository.GetByUserId(userId);
        }

        public IEnumerable<Task> GetTasksForToday(int userId)
        {
            return _taskRepository.GetByUserIdAndDate(userId, DateTime.Today);
        }

        public IEnumerable<Task> GetTasksCompletedByUserId(int userId)
        {
            return _taskRepository.GetByUserId(userId)
                .Where(t => t.Status == Domain.Enums.TaskStatus.Completed || t.IsCompleted)
                .ToList();
        }

        public Task AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title cannot be empty.");

            _taskRepository.Add(task);
            _taskRepository.SaveChanges();
            return task;
        }

        public void UpdateTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            _taskRepository.Update(task);
            _taskRepository.SaveChanges();
        }

        // Pending -> InProgress -> Completed
        public void ChangeTaskStatus(int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            if (task == null)
                return;

            switch (task.Status)
            {
                case Domain.Enums.TaskStatus.Pending:
                    task.Status = Domain.Enums.TaskStatus.InProgress;
                    break;
                case Domain.Enums.TaskStatus.InProgress:
                    task.Status = Domain.Enums.TaskStatus.Completed;
                    task.IsCompleted = true;
                    break;
                case Domain.Enums.TaskStatus.Completed:
                    return;
            }

            _taskRepository.Update(task);
            _taskRepository.SaveChanges();
        }

        public void DeleteTask(int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            if (task == null)
                return;

            _taskRepository.Delete(task);
            _taskRepository.SaveChanges();
        }
    }
}
