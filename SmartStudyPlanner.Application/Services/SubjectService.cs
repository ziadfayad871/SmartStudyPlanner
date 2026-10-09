using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using SmartStudyPlanner.Application.DTOs;
using SmartStudyPlanner.Domain.Enums;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

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

        public async Task<IEnumerable<Subject>> GetAllSubjects()
        {
            // Retrieve all subjects from the repository
            // get all async and return the result
            return await _subjectRepository.GetAllAsync();

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
                throw new ArgumentException("User ID Must be a positive integer.");

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
        public SubjectDto? GetSubjectDtoById(int subjectId)
        {
            var subject = _subjectRepository.GetById(subjectId);
            if (subject == null)
            {
                return null;
            }
            // Map the Subject entity to SubjectDto
            var subjectDto = new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Description = subject.Description,
                Difficulty = subject.Difficulty,
                Progress = subject.Progress,
                ExamDate = subject.ExamDate
            };
            return subjectDto;
        }
        public IEnumerable<Subject> GetHardSubjects()
        {
            var AllSubjects = _subjectRepository.GetAll();
            // Filter subjects with difficulty level "Hard"
            var hardSubjects = AllSubjects.Where(s => s.Difficulty == Difficulty.Hard);

            // You can perform additional operations with the hard subjects if needed
            return hardSubjects;
        }
        public SubjectDto? GetSubjectDtoByName(string subjectName)
        {
            var subject = _subjectRepository.GetAll().FirstOrDefault(s => s.Name == subjectName);
            if (subject == null)
            {
                return null;
            }
            // Map the Subject entity to SubjectDto
            var subjectDto = new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Description = subject.Description,
                Difficulty = subject.Difficulty,
                Progress = subject.Progress,
                ExamDate = subject.ExamDate
            };
            return subjectDto;
        }
        public Subject? GetFirstHardSubject()
        {
            var hardSubject = _subjectRepository.GetAll().FirstOrDefault(s => s.Difficulty == Difficulty.Hard);
            return hardSubject;
        }
        public bool HasHardSubjects()
        {
            var hardSubjects = _subjectRepository.GetAll().Any(s => s.Difficulty == Difficulty.Hard);
            return hardSubjects;
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            if (subject == null)
            {
                throw new ArgumentException("Subject cannot be null.");
            }
            await _subjectRepository.AddAsync(subject);
            await _subjectRepository.SaveChangesAsync();
        }

        public async Task<Subject?> GetSubjectByIdAsync(int subjectId)
        {
            var subject = await _subjectRepository.GetByIdAsync(subjectId);
            if (subject == null)
            {
                return null;
            }
            return subject;
        }
    }
}