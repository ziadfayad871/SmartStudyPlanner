using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        // Add any additional methods specific to Subject repository if needed
        IEnumerable<Subject> GetByUserId(int userId);
    }
}
