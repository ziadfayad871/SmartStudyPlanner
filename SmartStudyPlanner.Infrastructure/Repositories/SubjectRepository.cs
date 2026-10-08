using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using SmartStudyPlanner.Infrastructure.Data;

using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Infrastructure.Repositories
{
    public class SubjectRepository : Repository<Subject> , ISubjectRepository
    {
      

        public SubjectRepository(ApplicationDbContext context) : base(context)
        {
            
        }
        public IEnumerable<Subject> GetByUserId(int userId)
        {
            return _context.Set<Subject>().Where(s => s.UserId == userId).ToList();
        }

       
    }
}
