using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner.Application.DTOs;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Web.ViewModels;
using System.Security.Claims;

namespace SmartStudyPlanner.Web.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly IStudySessionService _sessionService;
        private readonly ISubjectService _subjectService;

        public CalendarController(IStudySessionService sessionService, ISubjectService subjectService)
        {
            _sessionService = sessionService;
            _subjectService = subjectService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        [HttpGet]
        public IActionResult Index(int? year, int? month, int? day)
        {
            var userId = GetCurrentUserId();
            var today = DateTime.Today;
            year ??= today.Year;
            month ??= today.Month;
            if (month < 1 || month > 12) month = today.Month;

            var first = new DateTime(year.Value, month.Value, 1);
            var daysInMonth = DateTime.DaysInMonth(year.Value, month.Value);
            // Week starts Saturday (offset from Saturday=0)
            int offset = ((int)first.DayOfWeek - (int)DayOfWeek.Saturday + 7) % 7;

            var sessions = _sessionService.GetAllStudySessionsForUser(userId).ToList();
            var byDate = sessions.GroupBy(s => s.Date.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            var weeks = new List<List<CalendarDay>>();
            var cursor = first.AddDays(-offset);
            for (int w = 0; w < 6; w++)
            {
                var week = new List<CalendarDay>();
                for (int d = 0; d < 7; d++)
                {
                    week.Add(new CalendarDay
                    {
                        Date = cursor,
                        InMonth = cursor.Month == month.Value,
                        IsToday = cursor.Date == today,
                        SessionCount = byDate.TryGetValue(cursor.Date, out var c) ? c : 0
                    });
                    cursor = cursor.AddDays(1);
                }
                weeks.Add(week);
                if (cursor.Month != month.Value && cursor > first.AddDays(daysInMonth)) break;
            }

            var selected = day.HasValue
                ? new DateTime(year.Value, month.Value, Math.Min(Math.Max(day.Value, 1), daysInMonth))
                : today.Month == month.Value && today.Year == year.Value ? today : first;

            var subjects = _subjectService.GetSubjectsByUserId(userId).ToList();

            var vm = new CalendarViewModel
            {
                Year = year.Value,
                Month = month.Value,
                Weeks = weeks,
                SelectedDate = selected,
                SelectedSessions = sessions
                    .Where(s => s.Date.Date == selected.Date)
                    .OrderBy(s => s.StartTime)
                    .ToList(),
                SubjectNames = subjects.ToDictionary(s => s.Id, s => s.Name),
                UpcomingExams = subjects
                    .Where(s => s.ExamDate != null && s.ExamDate.Value.Date >= today)
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
                    .ToList()
            };

            foreach (var week in vm.Weeks)
                foreach (var cd in week)
                    cd.IsSelected = cd.Date.Date == selected.Date;

            var prev = first.AddMonths(-1);
            var next = first.AddMonths(1);
            ViewBag.PrevYear = prev.Year; ViewBag.PrevMonth = prev.Month;
            ViewBag.NextYear = next.Year; ViewBag.NextMonth = next.Month;

            return View(vm);
        }
    }
}
