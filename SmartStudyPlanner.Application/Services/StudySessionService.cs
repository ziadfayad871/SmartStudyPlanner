using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using SmartStudyPlanner.Domain.Enums;
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
            studySession.Duration = studySession.EndTime - studySession.StartTime;
            _studySessionRepository.Add(studySession);
            _studySessionRepository.SaveChanges();
        }

        public void DeleteStudySession(StudySession studySession)
        {
            _studySessionRepository.Delete(studySession);
            _studySessionRepository.SaveChanges();
        }

        public IEnumerable<StudySession> GetAllStudySessionsForUser(int userId)
        {
            return _studySessionRepository.GetByUserId(userId);
        }

        public StudySession? GetStudySessionById(int id)
        {
            return _studySessionRepository.GetById(id);
        }

        public IEnumerable<StudySession> GetStudySessionsForUserOnDate(int userId, DateTime date)
        {
            return _studySessionRepository.GetByUserIdAndDate(userId, date);
        }

        public void MarkCompleted(int sessionId)
        {
            var session = _studySessionRepository.GetById(sessionId);
            if (session == null)
            {
                throw new ArgumentException("Session not found.", nameof(sessionId));
            }
            session.Status = SessionStatus.Completed;
            _studySessionRepository.Update(session);
            _studySessionRepository.SaveChanges();
        }

        public void UpdateStudySession(StudySession studySession)
        {
            if (studySession.StartTime >= studySession.EndTime)
            {
                throw new ArgumentException("Start time must be before end time.");
            }
            studySession.Duration = studySession.EndTime - studySession.StartTime;
            _studySessionRepository.Update(studySession);
            _studySessionRepository.SaveChanges();
        }
    }
}
