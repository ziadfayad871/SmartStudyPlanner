
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStudyPlanner.Application.Services
{
    public class StudyPlanningService : IStudyPlanningService
    {
        public int CalculateAvailableMinutes(Availability availability)
        {
            if (availability == null)
            {
                throw new ArgumentNullException(nameof(availability));
            }
            // check if AvailableFrom is greater than AvailableTo
            if (availability.AvailableFrom >= availability.AvailableTo)
            {
                throw new ArgumentException("AvailableFrom must be less than AvailableTo");
            }

            // calculate difference in minutes between AvailableFrom and AvailableTo
            TimeSpan difference = availability.AvailableTo - availability.AvailableFrom;

            return (int)difference.TotalMinutes;

        }

        public IEnumerable<Subject> GetAvailableSubjects(IEnumerable<Subject> subjects, int availableMinutes)
        {
            if (subjects == null)
            {
                throw new ArgumentNullException(nameof(subjects));
            }
            if (availableMinutes < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(availableMinutes));
            }
            if(subjects.Any() == false)
            {
                return Enumerable.Empty<Subject>();
            }
            // except subjects that have progress of 100%
            if (subjects.All(subject => subject.Progress == 100))
            {

return Enumerable.Empty<Subject>();
            }
            // filter subjects that have progress less than 100% and difficulty less than or equal to availableMinutes

return subjects
    .Where(subject =>
        subject.Progress < 100 &&
        subject.EstimatedMinutes <= availableMinutes)
    .OrderBy(subject => subject.ExamDate == null)
    .ThenBy(subject => subject.ExamDate)
    .ThenBy(subject => subject.Progress);

        }


        // Get subjects for planning, least progress first
        public IEnumerable<Subject> GetSubjectsForPlanning(
            IEnumerable<Subject> subjects)
        {
            if (subjects == null)
            {
                throw new ArgumentNullException(nameof(subjects));
            }

            return subjects.OrderBy(subject => subject.Progress);
        }
      
    }
}
