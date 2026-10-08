using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Services
{
    public class SubjectService : ISubjectService
       
    {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        public void AddSubject(Subject subject)
        {
            // Validate the subject (you can add more validation as needed)
            if (string.IsNullOrWhiteSpace(subject.Name))
            {
                throw new ArgumentException("Subject name cannot be empty.");
            }
            // Add the subject to the repository
            _subjectRepository.Add(subject);
            _subjectRepository.SaveChanges();
        }

        public void DeleteSubject(int subjectId)
        {

            // Retrieve the subject by ID
            var subject = _subjectRepository.GetById(subjectId);
            // Check if the subject exists
            if (subject != null)
            {
                // Delete the subject
                _subjectRepository.Delete(subject);
                _subjectRepository.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Subject with ID {subjectId} not found.");
            }
            }

        public IEnumerable<Subject> GetAllSubjects()
        {
            // Retrieve all subjects from the repository
            return _subjectRepository.GetAll();
        }

        public Subject? GetSubjectById(int subjectId)
        {
            // Retrieve the subject by ID
            return _subjectRepository.GetById(subjectId);
        }

        public IEnumerable<Subject> GetSubjectsByUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID cannot be null.");

            }
                      return _subjectRepository.GetByUserId(userId);
            

        }

        public void UpdateSubject(Subject subject)
        {
            if (subject == null)
            {
                throw new ArgumentException("Subject cannot be null.");
            }
            // Validate the subject (you can add more validation as needed)
            if (string.IsNullOrWhiteSpace(subject.Name))
            {
                throw new ArgumentException("Subject name cannot be empty.");
            }
            // Update the subject in the repository
            _subjectRepository.Update(subject);
            _subjectRepository.SaveChanges();
        }
    }
}
