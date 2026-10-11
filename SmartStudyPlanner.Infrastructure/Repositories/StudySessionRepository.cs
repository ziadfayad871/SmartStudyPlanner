using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using SmartStudyPlanner.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Infrastructure.Repositories
{
    public class StudySessionRepository : Repository<StudySession>,IStudySessionRepository
    {
        public StudySessionRepository(ApplicationDbContext context) : base(context)
        {
        }

        
        

        public IEnumerable<StudySession> GetByUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
            }
            return _context.StudySessions.Where(s => s.UserId == userId).ToList();
        }

        public IEnumerable<StudySession> GetByUserIdAndDate(int userId, DateTime date)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
            }
            if (date == DateTime.MinValue)
            {
                throw new ArgumentException("Date cannot be the default value.", nameof(date));
            }
            return _context.StudySessions.Where(s => s.UserId == userId && s.Date.Date == date.Date).ToList();
        }

        
    }
}
