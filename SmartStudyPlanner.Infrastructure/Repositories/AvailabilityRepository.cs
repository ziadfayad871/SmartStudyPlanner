using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using SmartStudyPlanner.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Infrastructure.Repositories
{
    public class AvailabilityRepository : Repository<Availability>, IAvailabilityRepository
    {
        public AvailabilityRepository(ApplicationDbContext context) : base(context)
        {
        }

        IEnumerable<Availability> IAvailabilityRepository.GetByUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("UserId must be greater than zero.", nameof(userId));
            }
            return _context.Availabilities.Where(a => a.UserId == userId).ToList();
        }

        IEnumerable<Availability> IAvailabilityRepository.GetByUserIdAndDate(int userId, DateTime date)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("UserId must be greater than zero.", nameof(userId));
            }
            if (date == DateTime.MinValue)
            {
                throw new ArgumentException("Date cannot be the default value.", nameof(date));
            }
            return _context.Availabilities.Where(a => a.UserId == userId && a.DayOfWeek == date.DayOfWeek).ToList();
        }
    }
}
