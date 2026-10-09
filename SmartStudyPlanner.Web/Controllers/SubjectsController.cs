using Microsoft.AspNetCore.Mvc;
using SmartStudyPlanner.Application.Services;
using SmartStudyPlanner.Domain.Entities;

namespace SmartStudyPlanner.Web.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ISubjectService _subjectService;
        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
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
    }
}
