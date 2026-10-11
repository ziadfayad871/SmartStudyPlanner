using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner.Application.DTOs;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Application.Services;
using SmartStudyPlanner.Domain.Entities;
using System.Security.Claims;

namespace SmartStudyPlanner.Web.Controllers
{
    [Authorize]
    public class SubjectsController : Controller
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var allSubjects = _subjectService.GetSubjectsByUserId(GetCurrentUserId()).ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                allSubjects = allSubjects
                    .Where(s => s.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                             || s.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.Search = search;
            return View(allSubjects);
        }

        public async Task<IActionResult> Details(int id)
        {
            var foundSubject = await _subjectService.GetSubjectByIdAsync(id);
            if (foundSubject == null || foundSubject.UserId != GetCurrentUserId())
            {
                return NotFound();
            }
            return View(foundSubject);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new SubjectDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectDto subjectDto)
        {
            subjectDto.UserId = GetCurrentUserId();

            if (!ModelState.IsValid)
            {
                return View(subjectDto);
            }

            await _subjectService.AddSubjectAsync(subjectDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var foundSubject = await _subjectService.GetSubjectByIdAsync(id);
            if (foundSubject == null || foundSubject.UserId != GetCurrentUserId())
            {
                return NotFound();
            }
            return View(foundSubject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubjectDto subjectDto)
        {
            if (id != subjectDto.Id)
            {
                return NotFound();
            }

            var existing = await _subjectService.GetSubjectByIdAsync(id);
            if (existing == null || existing.UserId != GetCurrentUserId())
            {
                return NotFound();
            }
            subjectDto.UserId = GetCurrentUserId();

            if (!ModelState.IsValid)
            {
                return View(subjectDto);
            }

            _subjectService.UpdateSubject(subjectDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var foundSubject = await _subjectService.GetSubjectByIdAsync(id);
            if (foundSubject == null || foundSubject.UserId != GetCurrentUserId())
            {
                return NotFound();
            }
            return View(foundSubject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existing = await _subjectService.GetSubjectByIdAsync(id);
            if (existing == null || existing.UserId != GetCurrentUserId())
            {
                return NotFound();
            }

            _subjectService.DeleteSubject(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
