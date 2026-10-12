
using SmartStudyPlanner.Domain.Entities;
using System.Collections.Generic;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IStudyPlanningService
    {
        int CalculateAvailableMinutes(Availability availability);

        IEnumerable<Subject> GetSubjectsForPlanning(
            IEnumerable<Subject> subjects);
        IEnumerable<Subject> GetAvailableSubjects (IEnumerable<Subject> subjects, int availableMinutes);

        StudyPlan GeneratePlan(int userId, string planName, DateTime startDate, DateTime endDate);
    }
    
}
