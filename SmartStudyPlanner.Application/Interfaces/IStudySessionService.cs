using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IStudySessionService 
    {
        // get all study sessions for a user
        IEnumerable<StudySession> GetAllStudySessionsForUser(int userId);
        // get a study session by id
        StudySession? GetStudySessionById(int id);
        // add a new study session
        void AddStudySession(StudySession studySession);
        // update an existing study session
        void UpdateStudySession(StudySession studySession);
        // delete an existing study session
        void DeleteStudySession(StudySession studySession);
        // get sessions for a user on a specific date
        IEnumerable<StudySession> GetStudySessionsForUserOnDate(int userId, DateTime date);
        // mark a session as completed
        void MarkCompleted(int sessionId);
    }
}
