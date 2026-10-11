using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IStudyPlanRepository : IRepository<StudyPlan>
    {
        System.Threading.Tasks.Task<StudyPlan> GetStudyPlanWithSessionsAsync(int studyPlanId);
        System.Threading.Tasks.Task<IEnumerable<StudyPlan>> GetAllStudyPlansWithSessionsAsync();
        Task<IEnumerable<StudyPlan>> GetStudyPlansByUserIdAsync(int userId);

    }
}
