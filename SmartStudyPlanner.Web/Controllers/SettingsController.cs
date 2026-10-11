using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner.Application.Interfaces;
using System.Security.Claims;

namespace SmartStudyPlanner.Web.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IUserService _userService;

        public SettingsController(IUserService userService)
        {
            _userService = userService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserById(GetCurrentUserId());
            if (user == null)
                return RedirectToAction("Login", "Account");

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string name, string email, string academicLevel, string? profilePictureUrl)
        {
            var user = await _userService.GetUserById(GetCurrentUserId());
            if (user == null)
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(string.Empty, "Name and email are required.");
                return View(user);
            }

            var existingEmail = _userService.GetUserByEmail(email);
            if (existingEmail != null && existingEmail.Id != user.Id)
            {
                ModelState.AddModelError(string.Empty, "This email is already in use.");
                return View(user);
            }

            user.Name = name;
            user.Email = email;
            user.AcademicLevel = academicLevel ?? string.Empty;
            user.ProfilePictureUrl = profilePictureUrl ?? string.Empty;

            await _userService.Update(user);

            return View(user);
        }
    }
}
