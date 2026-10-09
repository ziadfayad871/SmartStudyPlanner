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
        public async Task<IActionResult> Index()
        {
            var allSubjects = await _subjectService.GetAllSubjects();
           
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
        // create a new subject
        [HttpGet]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Users = _userRepository.GetAll().ToList();

            return View(new SubjectDto());
        }
       
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(SubjectDto subjectDto)
        {
            ViewBag.Users = _userRepository.GetAll().ToList();

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                ViewBag.Errors = errors;

                return View(subjectDto);
            }
            Console.WriteLine($"Received UserId: {subjectDto.UserId}");

            await _subjectService.AddSubjectAsync(subjectDto);

            return RedirectToAction(nameof(Index));
        }

        // edit a subject
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var foundSubject = await _subjectService.GetSubjectByIdAsync(id);
            if (foundSubject == null)
            {
                return NotFound();
            }
            // Pass the list of users to the view
            ViewBag.Users = _userRepository.GetAll().ToList();
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
            // Pass the list of users to the view
            ViewBag.Users = _userRepository.GetAll().ToList();

            if (ModelState.IsValid)
            {
                _subjectService.UpdateSubject(subjectDto);
                return RedirectToAction(nameof(Index));
            }
            return View(subjectDto);
        }
        // delete a subject
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(SubjectDto subjectDto)
        {
            _subjectService.DeleteSubject(subjectDto.Id);

            return RedirectToAction(nameof(Index));
        }
    }
}
