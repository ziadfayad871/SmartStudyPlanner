using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System.Security.Claims;

namespace SmartStudyPlanner.Web.Controllers
{
    [Authorize]
    public class AvailabilitiesController : Controller
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilitiesController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var items = _availabilityService.GetUserAvailability(GetCurrentUserId())
                .OrderBy(a => a.DayOfWeek)
                .ThenBy(a => a.AvailableFrom)
                .ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Availability { DayOfWeek = DayOfWeek.Saturday });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Availability availability)
        {
            availability.UserId = GetCurrentUserId();
            ModelState.Remove(nameof(Availability.User));

            if (!ModelState.IsValid)
                return View(availability);

            try
            {
                _availabilityService.SetAvailability(availability);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(availability);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _availabilityService.GetUserAvailability(GetCurrentUserId())
                .FirstOrDefault(a => a.Id == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Availability availability)
        {
            if (id != availability.Id)
                return NotFound();

            var existing = _availabilityService.GetUserAvailability(GetCurrentUserId())
                .FirstOrDefault(a => a.Id == id);
            if (existing == null)
                return NotFound();

            availability.UserId = GetCurrentUserId();
            ModelState.Remove(nameof(Availability.User));

            if (!ModelState.IsValid)
                return View(availability);

            try
            {
                _availabilityService.DeleteAvailability(id);
                _availabilityService.SetAvailability(availability);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(availability);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var item = _availabilityService.GetUserAvailability(GetCurrentUserId())
                .FirstOrDefault(a => a.Id == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var existing = _availabilityService.GetUserAvailability(GetCurrentUserId())
                .FirstOrDefault(a => a.Id == id);
            if (existing == null)
                return NotFound();

            _availabilityService.DeleteAvailability(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
