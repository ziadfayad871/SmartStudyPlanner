using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IAvailabilityRepository : IRepository<Availability>
    {
        IEnumerable<Availability> GetByUserId(int userId);
        IEnumerable<Availability> GetByUserIdAndDay(int userId, DayOfWeek day);
    }
}
