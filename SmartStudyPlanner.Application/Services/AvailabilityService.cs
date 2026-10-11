using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        public AvailabilityService(IAvailabilityRepository availabilityRepository)
        {
            _availabilityRepository = availabilityRepository;
        }

        public void DeleteAvailability(int availabilityId)
        {
            var availability = _availabilityRepository.GetById(availabilityId);
            if (availability != null)
            {
                _availabilityRepository.Delete(availability);
                _availabilityRepository.SaveChanges();
            }
        }

        public IEnumerable<Availability> GetUserAvailability(int userId)
        {
            return _availabilityRepository.GetByUserId(userId);
        }

        public IEnumerable<Availability> GetUserAvailabilityByDay(int userId, DayOfWeek day)
        {
            return _availabilityRepository.GetByUserIdAndDay(userId, day);
        }

        public void SetAvailability(Availability availability)
        {
            if (availability == null)
            {
                throw new ArgumentNullException(nameof(availability));
            }
            if (availability.AvailableFrom >= availability.AvailableTo)
            {
                throw new ArgumentException("AvailableFrom must be before AvailableTo.");
            }
            _availabilityRepository.Add(availability);
            _availabilityRepository.SaveChanges();
        }
    }
}
