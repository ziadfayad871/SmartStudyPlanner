using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartStudyPlanner.Application.Services
{
    public class StudyPlanService : IStudyPlanService
    {
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IStudySessionRepository _studySessionRepository;
        public StudyPlanService(IStudyPlanRepository studyPlanRepository, IStudySessionRepository studySessionRepository)
        {
            _studyPlanRepository = studyPlanRepository;
            _studySessionRepository = studySessionRepository;
        }
        public void AddStudyPlan(StudyPlan studyPlan)
        {
            if (studyPlan == null)
            {
                throw new ArgumentNullException(nameof(studyPlan));
            }
            _studyPlanRepository.Add(studyPlan);
            _studyPlanRepository.SaveChanges();
        }

        public void DeleteStudyPlan(StudyPlan studyPlan)
        {
            if (studyPlan == null)
            {
                throw new ArgumentNullException(nameof(studyPlan));
            }
            _studyPlanRepository.Delete(studyPlan);
            _studyPlanRepository.SaveChanges();
        }

        public Task<IEnumerable<StudyPlan>> GetAllStudyPlansWithSessionsAsync()
        {
            return _studyPlanRepository.GetAllStudyPlansWithSessionsAsync();
        }

        public Task<IEnumerable<StudyPlan>> GetStudyPlansByUserIdAsync(int userId)
        {
            return _studyPlanRepository.GetStudyPlansByUserIdAsync(userId);
        }

        public Task<StudyPlan> GetStudyPlanWithSessionsAsync(int studyPlanId)
        {
            return _studyPlanRepository.GetStudyPlanWithSessionsAsync(studyPlanId);
        }

        public void RemoveSessionFromStudyPlan(int sessionId)
        {
            var studySession = _studySessionRepository.GetById(sessionId);
            if (studySession == null)
            {
                throw new ArgumentException("Session not found.", nameof(sessionId));
            }
            studySession.StudyPlanId = null;
            _studySessionRepository.Update(studySession);
            _studySessionRepository.SaveChanges();
        }
    }
}
