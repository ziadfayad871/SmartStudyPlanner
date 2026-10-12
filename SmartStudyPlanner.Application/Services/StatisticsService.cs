using System;
using System.Linq;
using SmartStudyPlanner.Application.DTOs;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using SmartStudyPlanner.Domain.Enums;
using Task = System.Threading.Tasks.Task;

namespace SmartStudyPlanner.Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IStudySessionRepository _sessionRepository;
        private readonly ITaskRepository _taskRepository;

        public StatisticsService(
            ISubjectRepository subjectRepository,
            IStudySessionRepository sessionRepository,
            ITaskRepository taskRepository)
        {
            _subjectRepository = subjectRepository;
            _sessionRepository = sessionRepository;
            _taskRepository = taskRepository;
        }

        public Task<StatisticsDto> GetStatistics(int userId)
        {
            var today = DateTime.Today;
            var subjects = _subjectRepository.GetByUserId(userId).ToList();
            var completed = _sessionRepository.GetByUserId(userId)
                .Where(s => s.Status == SessionStatus.Completed)
                .ToList();
            var subjectById = subjects.ToDictionary(s => s.Id);

            var last7Days = Enumerable.Range(0, 7)
                .Select(i => today.AddDays(-6 + i))
                .Select(d => new StudyMinutesByDayDto
                {
                    Date = d,
                    StudyMinutesFor7Days = (int)completed
                        .Where(s => s.Date.Date == d)
                        .Sum(s => s.Duration.TotalMinutes)
                })
                .ToList();

            var dto = new StatisticsDto
            {
                TotalStudyMinutes = (int)completed.Sum(s => s.Duration.TotalMinutes),
                SessionsCompleted = completed.Count,
                TasksCompleted = _taskRepository.GetByUserId(userId)
                    .Count(t => t.Status == Domain.Enums.TaskStatus.Completed),
                AverageProgress = subjects.Count == 0
                    ? 0
                    : Math.Round(subjects.Average(s => s.Progress), 1),
                SubjectProgress = subjects
                    .OrderByDescending(s => s.Progress)
                    .Select(s => new SubjectProgressDto
                    {
                        SubjectName = s.Name,
                        ProgressPercentage = s.Progress,
                        Color = s.Color
                    })
                    .ToList(),
                StudyMinutesByDay = last7Days,
                StudyMinutesBySubject = completed
                    .GroupBy(s => s.SubjectId)
                    .Select(g => new StudyMinutesBySubjectDto
                    {
                        SubjectName = subjectById.TryGetValue(g.Key, out var sub) ? sub.Name : string.Empty,
                        StudyMinutes = (int)g.Sum(s => s.Duration.TotalMinutes)
                    })
                    .OrderByDescending(x => x.StudyMinutes)
                    .ToList()
            };

            return Task.FromResult(dto);
        }
    }
}
