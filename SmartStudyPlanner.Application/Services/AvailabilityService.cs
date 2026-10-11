using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Services
{
    public class AvailabilityService : IAvailabilityService
    {
       private readonly IRepository<StudySession> _studySessionRepository;
        public AvailabilityService(IRepository<StudySession> studySessionRepository)
        {
            _studySessionRepository = studySessionRepository;
        }
        public void DeleteAvailability(int AvailabilityId)
        {
           var studySession = _studySessionRepository.GetById(AvailabilityId);
            if (studySession != null)
            {
                _studySessionRepository.Delete(studySession);
            }
        }

        public IEnumerable<StudySession>  GetUserAvailability(int userId)
        {
            return _studySessionRepository.GetAll()
                .Where(s => s.UserId == userId)
                .ToList();

        }

        public void SetAvailability(IEnumerable<StudySession> availability)
        {
            foreach (var session in availability)
            {
                _studySessionRepository.Add(session);
            }
        }
    }
}
