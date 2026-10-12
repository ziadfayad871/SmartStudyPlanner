using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IStudyPlanService
    {
        Task<IEnumerable<StudyPlan>> GetAllStudyPlansWithSessionsAsync();
        Task<IEnumerable<StudyPlan>> GetStudyPlansByUserIdAsync(int userId);
        Task<StudyPlan> GetStudyPlanWithSessionsAsync(int studyPlanId);
        void AddStudyPlan(StudyPlan studyPlan);
        void DeleteStudyPlan(StudyPlan studyPlan);
        void RemoveSessionFromStudyPlan(int sessionId);
        // generate plan : 
        Task<StudyPlan> GenerateStudyPlanAsync(int userId,string name, DateTime startDate, DateTime endDate);
    }
}
