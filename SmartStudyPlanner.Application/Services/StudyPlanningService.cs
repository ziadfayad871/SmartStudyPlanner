
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStudyPlanner.Application.Services
{
    public class StudyPlanningService : IStudyPlanningService
    {
        private const int MaxMinutesPerSession = 60;
        private const int MinMinutesPerSession = 15;

        private readonly IStudyPlanRepository _planRepository;
        private readonly IStudySessionRepository _sessionRepository;
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly ISubjectRepository _subjectRepository;

        public StudyPlanningService(
            IStudyPlanRepository planRepository,
            IStudySessionRepository sessionRepository,
            IAvailabilityRepository availabilityRepository,
            ISubjectRepository subjectRepository)
        {
            _planRepository = planRepository;
            _sessionRepository = sessionRepository;
            _availabilityRepository = availabilityRepository;
            _subjectRepository = subjectRepository;
        }

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

        // Generate a study plan with sessions distributed over the date range
        public StudyPlan GeneratePlan(int userId, string planName, DateTime startDate, DateTime endDate)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be positive.", nameof(userId));
            if (string.IsNullOrWhiteSpace(planName))
                throw new ArgumentException("Plan name is required.", nameof(planName));

            startDate = startDate.Date;
            endDate = endDate.Date;
            if (endDate < startDate)
                throw new ArgumentException("End date must be on or after start date.");

            var plan = new StudyPlan
            {
                UserId = userId,
                Name = planName.Trim(),
                Description = $"Auto-generated plan ({startDate:MMM d} - {endDate:MMM d, yyyy}).",
                StartDate = startDate,
                EndDate = endDate,
                CreatedAt = DateTime.UtcNow
            };
            _planRepository.Add(plan);
            _planRepository.SaveChanges();

            var subjects = _subjectRepository.GetByUserId(userId)
                .Where(s => s.Progress < 100)
                .OrderBy(s => s.ExamDate == null)
                .ThenBy(s => s.ExamDate)
                .ThenBy(s => s.Progress)
                .ThenByDescending(s => s.Difficulty)
                .ToList();
            if (!subjects.Any())
                return plan;

            var remaining = subjects.ToDictionary(s => s.Id, s => Math.Max(s.EstimatedMinutes, MinMinutesPerSession));
            var availabilities = _availabilityRepository.GetByUserId(userId).ToList();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var daySlots = availabilities
                    .Where(a => a.DayOfWeek == date.DayOfWeek)
                    .OrderBy(a => a.AvailableFrom)
                    .ToList();
                if (!daySlots.Any())
                    continue;

                var existing = _sessionRepository.GetByUserIdAndDate(userId, date).ToList();

                foreach (var slot in daySlots)
                {
                    var cursor = slot.AvailableFrom;
                    var slotEnd = slot.AvailableTo;

                    while ((slotEnd - cursor).TotalMinutes >= MinMinutesPerSession)
                    {
                        var subject = subjects.FirstOrDefault(s => remaining[s.Id] > 0);
                        if (subject == null)
                            return SaveAndReturn(plan);

                        var freeMinutes = FreeMinutesInSlot(cursor, slotEnd, existing);
                        if (freeMinutes < MinMinutesPerSession)
                            break;

                        var chunk = Math.Min(MaxMinutesPerSession, Math.Min(freeMinutes, remaining[subject.Id]));
                        if (chunk < MinMinutesPerSession)
                        {
                            remaining[subject.Id] = 0;
                            continue;
                        }

                        var session = new StudySession
                        {
                            UserId = userId,
                            SubjectId = subject.Id,
                            StudyPlanId = plan.Id,
                            Date = date,
                            StartTime = cursor,
                            EndTime = cursor.AddMinutes(chunk),
                            Duration = TimeSpan.FromMinutes(chunk),
                            Status = Domain.Enums.SessionStatus.Scheduled
                        };
                        _sessionRepository.Add(session);
                        existing.Add(session);

                        remaining[subject.Id] -= chunk;
                        cursor = cursor.AddMinutes(chunk);
                    }
                }
            }

            return SaveAndReturn(plan);
        }

        private StudyPlan SaveAndReturn(StudyPlan plan)
        {
            _sessionRepository.SaveChanges();
            return plan;
        }

        // Minutes free in [from, to) after subtracting overlapping existing sessions
        private static int FreeMinutesInSlot(TimeOnly from, TimeOnly to, List<StudySession> existing)
        {
            var total = (int)(to - from).TotalMinutes;
            foreach (var s in existing.OrderBy(e => e.StartTime))
            {
                var overlapStart = s.StartTime > from ? s.StartTime : from;
                var overlapEnd = s.EndTime < to ? s.EndTime : to;
                if (overlapEnd > overlapStart)
                    total -= (int)(overlapEnd - overlapStart).TotalMinutes;
            }
            return Math.Max(total, 0);
        }
    }
}
