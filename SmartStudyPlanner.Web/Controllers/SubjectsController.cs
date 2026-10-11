using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner.Application.DTOs;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Application.Services;
using SmartStudyPlanner.Domain.Entities;

namespace SmartStudyPlanner.Web.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService, IRepository<User> userRepository)
        {
            _subjectService = subjectService;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var allSubjects = (await _subjectService.GetAllSubjects()).ToList();

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
            if (foundSubject == null)
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
            var defaultUser = _userRepository.GetAll().FirstOrDefault();
            if (defaultUser == null)
            {
                ModelState.AddModelError(string.Empty, "No user found. Please seed a user first.");
                return View(subjectDto);
            }
            subjectDto.UserId = defaultUser.Id;

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
            if (foundSubject == null)
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
            if (existing == null)
            {
                return NotFound();
            }
            subjectDto.UserId = existing.UserId;

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
            if (foundSubject == null)
            {
                return NotFound();
            }
            return View(foundSubject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _subjectService.DeleteSubject(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
