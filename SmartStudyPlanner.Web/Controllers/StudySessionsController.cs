using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner.Application.DTOs;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using SmartStudyPlanner.Domain.Enums;
using System.Security.Claims;

namespace SmartStudyPlanner.Web.Controllers
{
    [Authorize]
    public class StudySessionsController : Controller
    {
        private readonly IStudySessionService _sessionService;
        private readonly ISubjectService _subjectService;
        private readonly IStudyPlanService _planService;

        public StudySessionsController(
            IStudySessionService sessionService,
            ISubjectService subjectService,
            IStudyPlanService planService)
        {
            _sessionService = sessionService;
            _subjectService = subjectService;
            _planService = planService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        [HttpGet]
        public IActionResult Index(string? filter)
        {
            var userId = GetCurrentUserId();
            var sessions = _sessionService.GetAllStudySessionsForUser(userId).ToList();
            var today = DateTime.Today;

            ViewBag.AllCount = sessions.Count;
            ViewBag.TodayCount = sessions.Count(s => s.Date.Date == today && s.Status != SessionStatus.Completed);
            ViewBag.CompletedCount = sessions.Count(s => s.Status == SessionStatus.Completed);

            filter = (filter ?? "all").ToLower();
            sessions = filter switch
            {
                "today" => sessions.Where(s => s.Date.Date == today).ToList(),
                "upcoming" => sessions.Where(s => s.Date.Date > today && s.Status != SessionStatus.Completed).ToList(),
                "completed" => sessions.Where(s => s.Status == SessionStatus.Completed).ToList(),
                _ => sessions
            };

            ViewBag.Filter = filter;
            ViewBag.Subjects = _subjectService.GetSubjectsByUserId(userId)
                .ToDictionary(s => s.Id, s => s.Name);
            return View(sessions.OrderBy(s => s.Date).ThenBy(s => s.StartTime).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = GetCurrentUserId();
            ViewBag.Subjects = _subjectService.GetSubjectsByUserId(userId).ToList();
            ViewBag.Plans = (await _planService.GetStudyPlansByUserIdAsync(userId)).ToList();
            return View(new StudySession { Date = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudySession session)
        {
            var userId = GetCurrentUserId();
            session.UserId = userId;
            ModelState.Remove(nameof(StudySession.Subject));
            ModelState.Remove(nameof(StudySession.User));

            if (!ModelState.IsValid)
            {
                ViewBag.Subjects = _subjectService.GetSubjectsByUserId(userId).ToList();
                ViewBag.Plans = (await _planService.GetStudyPlansByUserIdAsync(userId)).ToList();
                return View(session);
            }

            try
            {
                _sessionService.AddStudySession(session);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Subjects = _subjectService.GetSubjectsByUserId(userId).ToList();
                ViewBag.Plans = (await _planService.GetStudyPlansByUserIdAsync(userId)).ToList();
                return View(session);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var session = _sessionService.GetStudySessionById(id);
            if (session == null || session.UserId != GetCurrentUserId())
                return NotFound();

            ViewBag.Subjects = _subjectService.GetSubjectsByUserId(GetCurrentUserId()).ToList();
            ViewBag.Plans = (await _planService.GetStudyPlansByUserIdAsync(GetCurrentUserId())).ToList();
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudySession session)
        {
            if (id != session.Id)
                return NotFound();

            var existing = _sessionService.GetStudySessionById(id);
            if (existing == null || existing.UserId != GetCurrentUserId())
                return NotFound();

            session.UserId = GetCurrentUserId();
            ModelState.Remove(nameof(StudySession.Subject));
            ModelState.Remove(nameof(StudySession.User));

            if (!ModelState.IsValid)
            {
                ViewBag.Subjects = _subjectService.GetSubjectsByUserId(GetCurrentUserId()).ToList();
                ViewBag.Plans = (await _planService.GetStudyPlansByUserIdAsync(GetCurrentUserId())).ToList();
                return View(session);
            }

            try
            {
                _sessionService.UpdateStudySession(session);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Subjects = _subjectService.GetSubjectsByUserId(GetCurrentUserId()).ToList();
                ViewBag.Plans = (await _planService.GetStudyPlansByUserIdAsync(GetCurrentUserId())).ToList();
                return View(session);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var session = _sessionService.GetStudySessionById(id);
            if (session == null || session.UserId != GetCurrentUserId())
                return NotFound();

            return View(session);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var existing = _sessionService.GetStudySessionById(id);
            if (existing == null || existing.UserId != GetCurrentUserId())
                return NotFound();

            _sessionService.DeleteStudySession(existing);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkComplete(int id)
        {
            var existing = _sessionService.GetStudySessionById(id);
            if (existing == null || existing.UserId != GetCurrentUserId())
                return NotFound();

            _sessionService.MarkCompleted(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
