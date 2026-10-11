using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IAvailabilityService
    {
        // get availability for a user :
        IEnumerable<Availability> GetUserAvailability(int userId);
        // get availability for a user on a specific day :
        IEnumerable<Availability> GetUserAvailabilityByDay(int userId, DayOfWeek day);
        // set availability :
        void SetAvailability(Availability availability);
        // delete availability :
        void DeleteAvailability(int availabilityId);


    }
}
