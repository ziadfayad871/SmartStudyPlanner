using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Application.Services;
using System.Security.Claims;

namespace SmartStudyPlanner.Web.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;
        private readonly IStudySessionService _sessionService;

        public SettingsController(
            IUserService userService,
            ITaskService taskService,
            IStudySessionService sessionService)
        {
            _userService = userService;
            _taskService = taskService;
            _sessionService = sessionService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? tab)
        {
            var user = await _userService.GetUserById(GetCurrentUserId());
            if (user == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Tab = string.IsNullOrWhiteSpace(tab) ? "profile" : tab.ToLower();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string name, string email, string academicLevel, string? profilePictureUrl, IFormFile? photo)
        {
            var user = await _userService.GetUserById(GetCurrentUserId());
            if (user == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Tab = "profile";

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

            if (photo != null && photo.Length > 0)
            {
                if (!photo.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(string.Empty, "Please upload an image file.");
                    return View(user);
                }
                if (photo.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError(string.Empty, "Image must be smaller than 2 MB.");
                    return View(user);
                }
                using (var ms = new MemoryStream())
                {
                    await photo.CopyToAsync(ms);
                    user.ProfilePicture = ms.ToArray();
                    user.ProfilePictureContentType = photo.ContentType;
                }
                user.ProfilePictureUrl = string.Empty;
            }

            await _userService.Update(user);
            TempData["Success"] = "Profile updated successfully.";

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Photo()
        {
            var user = await _userService.GetUserById(GetCurrentUserId());
            if (user?.ProfilePicture == null || user.ProfilePicture.Length == 0)
                return NotFound();

            return File(user.ProfilePicture, user.ProfilePictureContentType ?? "image/jpeg");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var user = await _userService.GetUserById(GetCurrentUserId());
            if (user == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Tab = "account";

            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError(string.Empty, "Please fill in all password fields.");
                return View("Index", user);
            }
            if (!_userService.CheckPassword(user, currentPassword))
            {
                ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                return View("Index", user);
            }
            if (newPassword.Length < 6)
            {
                ModelState.AddModelError(string.Empty, "New password must be at least 6 characters.");
                return View("Index", user);
            }
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "New passwords do not match.");
                return View("Index", user);
            }

            user.PasswordHash = UserService.HashPassword(newPassword);
            await _userService.Update(user);
            TempData["Success"] = "Password changed successfully.";

            return View("Index", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNotifications(bool emailNotifications = false, bool studyReminders = false, bool examAlerts = false)
        {
            var user = await _userService.GetUserById(GetCurrentUserId());
            if (user == null)
                return RedirectToAction("Login", "Account");

            user.EmailNotifications = emailNotifications;
            user.StudyReminders = studyReminders;
            user.ExamAlerts = examAlerts;
            await _userService.Update(user);

            TempData["Success"] = "Notification preferences saved.";
            ViewBag.Tab = "notifications";
            return View("Index", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAppearance(string theme, string language)
        {
            var user = await _userService.GetUserById(GetCurrentUserId());
            if (user == null)
                return RedirectToAction("Login", "Account");

            user.Theme = theme == "dark" ? "dark" : "light";
            await _userService.Update(user);

            Response.Cookies.Append("ssp-theme", user.Theme,
                new CookieOptions { Path = "/", MaxAge = TimeSpan.FromDays(365) });
            if (language == "ar" || language == "en")
                Response.Cookies.Append("ssp-lang", language,
                    new CookieOptions { Path = "/", MaxAge = TimeSpan.FromDays(365) });

            TempData["Success"] = "Appearance preferences saved.";
            ViewBag.Tab = "appearance";
            return View("Index", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePrivacy(bool profilePublic = false, bool showEmail = false)
        {
            var user = await _userService.GetUserById(GetCurrentUserId());
            if (user == null)
                return RedirectToAction("Login", "Account");

            user.ProfilePublic = profilePublic;
            user.ShowEmail = showEmail;
            await _userService.Update(user);

            TempData["Success"] = "Privacy preferences saved.";
            ViewBag.Tab = "privacy";
            return View("Index", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(string confirmEmail)
        {
            var userId = GetCurrentUserId();
            var user = await _userService.GetUserById(userId);
            if (user == null)
                return RedirectToAction("Login", "Account");

            if (!string.Equals(confirmEmail?.Trim(), user.Email, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "Email does not match your account email.");
                ViewBag.Tab = "account";
                return View("Index", user);
            }

            foreach (var task in _taskService.GetAllByUserId(userId).ToList())
                _taskService.DeleteTask(task.Id);
            foreach (var session in _sessionService.GetAllStudySessionsForUser(userId).ToList())
                _sessionService.DeleteStudySession(session);

            await _userService.Delete(userId);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
