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
        public async Task<IActionResult> Index(string name, string email, string academicLevel, string? profilePictureUrl, IFormFile? photo)
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
    }
}
