using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System.Security.Claims;

namespace SmartStudyPlanner.Web.Controllers
{
    [Authorize]
    public class StudyPlansController : Controller
    {
        private readonly IStudyPlanService _planService;
        private readonly IStudySessionService _sessionService;

        public StudyPlansController(IStudyPlanService planService, IStudySessionService sessionService)
        {
            _planService = planService;
            _sessionService = sessionService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var plans = await _planService.GetStudyPlansByUserIdAsync(GetCurrentUserId());
            return View(plans.OrderByDescending(p => p.CreatedAt).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            StudyPlan plan;
            try
            {
                plan = await _planService.GetStudyPlanWithSessionsAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            if (plan.UserId != GetCurrentUserId())
                return NotFound();

            ViewBag.FreeSessions = _sessionService
                .GetAllStudySessionsForUser(GetCurrentUserId())
                .Where(s => s.StudyPlanId == null)
                .OrderBy(s => s.Date)
                .ToList();
            return View(plan);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new StudyPlan
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(7)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudyPlan plan)
        {
            plan.UserId = GetCurrentUserId();
            plan.CreatedAt = DateTime.UtcNow;
            ModelState.Remove(nameof(StudyPlan.User));

            if (!ModelState.IsValid)
                return View(plan);

            _planService.AddStudyPlan(plan);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            StudyPlan plan;
            try
            {
                plan = await _planService.GetStudyPlanWithSessionsAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            if (plan.UserId != GetCurrentUserId())
                return NotFound();

            return View(plan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            StudyPlan plan;
            try
            {
                plan = await _planService.GetStudyPlanWithSessionsAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            if (plan.UserId != GetCurrentUserId())
                return NotFound();

            _planService.DeleteStudyPlan(plan);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSession(int planId, int sessionId)
        {
            StudyPlan plan;
            try
            {
                plan = await _planService.GetStudyPlanWithSessionsAsync(planId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            if (plan.UserId != GetCurrentUserId())
                return NotFound();

            var session = _sessionService.GetStudySessionById(sessionId);
            if (session == null || session.UserId != GetCurrentUserId())
                return NotFound();

            session.StudyPlanId = planId;
            _sessionService.UpdateStudySession(session);
            return RedirectToAction(nameof(Details), new { id = planId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSession(int planId, int sessionId)
        {
            StudyPlan plan;
            try
            {
                plan = await _planService.GetStudyPlanWithSessionsAsync(planId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            if (plan.UserId != GetCurrentUserId())
                return NotFound();

            var session = _sessionService.GetStudySessionById(sessionId);
            if (session == null || session.UserId != GetCurrentUserId() || session.StudyPlanId != planId)
                return NotFound();

            _planService.RemoveSessionFromStudyPlan(sessionId);
            return RedirectToAction(nameof(Details), new { id = planId });
        }
    }
}
