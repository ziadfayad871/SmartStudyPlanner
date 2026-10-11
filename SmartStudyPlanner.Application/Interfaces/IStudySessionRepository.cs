using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IStudySessionRepository : IRepository<StudySession>
    {
        // get by user id
        IEnumerable<StudySession> GetByUserId(int userId);
        // GetByUserIdAndDate 
        IEnumerable<StudySession> GetByUserIdAndDate(int userId, DateTime date);


    }
}
