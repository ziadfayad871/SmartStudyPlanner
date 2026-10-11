using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Services
{
    public class StudySessionService : IStudySessionService
    {
        private readonly IStudySessionRepository _studySessionRepository;
        public StudySessionService(IStudySessionRepository studySessionRepository)
        {
            _studySessionRepository = studySessionRepository;
        }
        public void AddStudySession(StudySession studySession)
        {
            if (studySession == null)
            {
                throw new ArgumentNullException(nameof(studySession));
            }
            if (studySession.StartTime >= studySession.EndTime)
            {
                throw new ArgumentException("Start time must be before end time.");
            }
            _studySessionRepository.Add(studySession);
        }

        public void DeleteStudySession(StudySession studySession)
        {
            _studySessionRepository.Delete(studySession);
        }

        public IEnumerable<StudySession> GetAllStudySessionsForUser(int userId)
        {
            return _studySessionRepository.GetAll()
                .Where(s => s.UserId == userId)
                .ToList();
        }

        public StudySession? GetStudySessionById(int id)
        {
            return _studySessionRepository.GetById(id);
        }

        public IEnumerable<StudySession> GetStudySessionsForUserOnDate(int userId, DateTime date)
        {
            return _studySessionRepository.GetAll()
                .Where(s => s.UserId == userId && s.Date == date)
                .ToList();
        }

        public void UpdateStudySession(StudySession studySession)
        {
            _studySessionRepository.Update(studySession);
        }
    }
}
