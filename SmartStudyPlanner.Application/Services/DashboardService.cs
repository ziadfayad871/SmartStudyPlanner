using System;
using System.Linq;
using SmartStudyPlanner.Application.DTOs;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Enums;
using TaskStatus = SmartStudyPlanner.Domain.Enums.TaskStatus;

namespace SmartStudyPlanner.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IStudySessionRepository _sessionRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        

        public DashboardService(
            ISubjectRepository subjectRepository,
            IStudySessionRepository sessionRepository,
            ITaskRepository taskRepository,
            IUserRepository userRepository)
        {
            _subjectRepository = subjectRepository;
            _sessionRepository = sessionRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public DashboardDto GetDashboard(int userId)
        {
            var today = DateTime.Today;
            var user = _userRepository.GetById(userId);
            var subjects = _subjectRepository.GetByUserId(userId).ToList();
            var todaySessions = _sessionRepository.GetByUserIdAndDate(userId, today).ToList();

            var completedToday = todaySessions.Count(s => s.Status == SessionStatus.Completed);
            var studiedMinutes = (int)todaySessions
                .Where(s => s.Status == SessionStatus.Completed)
                .Sum(s => s.Duration.TotalMinutes);
            var totalMinutes = (int)todaySessions.Sum(s => s.Duration.TotalMinutes);

            var upcoming = subjects
                .Where(s => s.ExamDate != null && s.ExamDate.Value.Date >= today && s.Progress < 100)
                .OrderBy(s => s.ExamDate)
                .Take(4)
                .Select(s => new UpcomingExamDto
                {
                    SubjectId = s.Id,
                    SubjectName = s.Name,
                    ExamDate = s.ExamDate!.Value,
                    DaysLeft = (s.ExamDate!.Value.Date - today).Days,
                    Difficulty = s.Difficulty
                })
                .ToList();

            var nextExam = subjects
                .Where(s => s.ExamDate != null && s.ExamDate.Value.Date >= today)
                .OrderBy(s => s.ExamDate)
                .FirstOrDefault();

            var subjectNames = subjects.ToDictionary(s => s.Id, s => s.Name);

            return new DashboardDto
            {
                UserName = user?.Name ?? string.Empty,
                TodayDate = today,
                SubjectsCount = subjects.Count,
                TodaySessionsCount = todaySessions.Count,
                DaysUntilNextExam = nextExam?.ExamDate != null
                    ? (nextExam.ExamDate.Value.Date - today).Days
                    : 0,
                TodayProgressPercent = todaySessions.Count == 0
                    ? 0
                    : Math.Round(completedToday * 100.0 / todaySessions.Count, 1),
                StudiedMinutesToday = studiedMinutes,
                RemainingMinutesToday = Math.Max(totalMinutes - studiedMinutes, 0),
                CompletedTasksCount = _taskRepository.GetByUserId(userId)
                    .Count(t => t.Status == TaskStatus.Completed),
                UpcomingExams = upcoming,
                TodayPlans = todaySessions
                    .OrderBy(s => s.StartTime)
                    .Select(s => new TodayPlan
                    {
                        SessionId = s.Id,
                        SubjectName = subjectNames.TryGetValue(s.SubjectId, out var name) ? name : string.Empty,
                        Date = s.Date,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        Status = s.Status
                    })
                    .ToList()
            };
        }
    }
}
