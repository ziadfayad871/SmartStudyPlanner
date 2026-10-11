using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IAvailabilityService
    {
        // get availability for a user : 
        IEnumerable<StudySession> GetUserAvailability(int userId);
        // set availability : 
        void SetAvailability( IEnumerable<StudySession> availability);
        // delete availability for all : 
        void DeleteAvailability(int AvailabilityId);


    }
}
