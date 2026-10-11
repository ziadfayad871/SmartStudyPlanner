using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner.Application.Interfaces;
using System.Security.Claims;
using Task = SmartStudyPlanner.Domain.Entities.Task;
using TaskStatus = SmartStudyPlanner.Domain.Enums.TaskStatus;
using Priority = SmartStudyPlanner.Domain.Enums.Priority;

namespace SmartStudyPlanner.Web.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ISubjectService _subjectService;

        public TasksController(ITaskService taskService, ISubjectService subjectService)
        {
            _taskService = taskService;
            _subjectService = subjectService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        public IActionResult Index(string? filter)
        {
            var userId = GetCurrentUserId();
            var allTasks = _taskService.GetAllByUserId(userId).ToList();

            var todayTasks = allTasks
                .Where(t => t.DueDate.Date == DateTime.Today && t.Status != TaskStatus.Completed)
                .ToList();
            var completedTasks = allTasks
                .Where(t => t.Status == TaskStatus.Completed)
                .ToList();

            ViewBag.Filter = filter ?? "all";
            ViewBag.AllCount = allTasks.Count;
            ViewBag.TodayCount = todayTasks.Count;
            ViewBag.CompletedCount = completedTasks.Count;

            var tasks = filter switch
            {
                "today" => todayTasks,
                "completed" => completedTasks,
                _ => allTasks
            };

            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = GetCurrentUserId();
            ViewBag.Subjects = _subjectService.GetSubjectsByUserId(userId).ToList();
            return View(new Task());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Task task)
        {
            var userId = GetCurrentUserId();
            task.UserId = userId;

            if (!ModelState.IsValid)
            {
                ViewBag.Subjects = _subjectService.GetSubjectsByUserId(userId).ToList();
                return View(task);
            }

            _taskService.AddTask(task);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null || task.UserId != GetCurrentUserId())
                return NotFound();

            ViewBag.Subjects = _subjectService.GetSubjectsByUserId(GetCurrentUserId()).ToList();
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Task task)
        {
            if (id != task.Id)
                return NotFound();

            var existing = _taskService.GetTaskById(id);
            if (existing == null || existing.UserId != GetCurrentUserId())
                return NotFound();

            task.UserId = existing.UserId;

            if (!ModelState.IsValid)
            {
                ViewBag.Subjects = _subjectService.GetSubjectsByUserId(GetCurrentUserId()).ToList();
                return View(task);
            }

            existing.Title = task.Title;
            existing.SubjectId = task.SubjectId;
            existing.Priority = task.Priority;
            existing.DueDate = task.DueDate;
            existing.EstimatedMinutes = task.EstimatedMinutes;
            existing.Status = task.Status;
            existing.IsCompleted = task.Status == TaskStatus.Completed;

            _taskService.UpdateTask(existing);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null || task.UserId != GetCurrentUserId())
                return NotFound();

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null || task.UserId != GetCurrentUserId())
                return NotFound();

            _taskService.DeleteTask(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatus(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null || task.UserId != GetCurrentUserId())
                return NotFound();

            _taskService.ChangeTaskStatus(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
