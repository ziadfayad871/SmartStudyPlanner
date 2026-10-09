
using SmartStudyPlanner.Application.DTOs;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using SmartStudyPlanner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStudyPlanner.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        // Convert Entity to DTO
        private static SubjectDto MapToDto(Subject subject)
        {
            return new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Description = subject.Description,
                Difficulty = subject.Difficulty,
                Progress = subject.Progress,
                ExamDate = subject.ExamDate,
                UserId = subject.UserId
            };
        }

        // Get all subjects
        public async Task<IEnumerable<SubjectDto>> GetAllSubjects()
        {
            var subjects = await _subjectRepository.GetAllAsync();

            return subjects.Select(MapToDto).ToList();
        }

        // Get one subject by ID
        public async Task<SubjectDto?> GetSubjectByIdAsync(int subjectId)
        {
            var subject = await _subjectRepository.GetByIdAsync(subjectId);

            if (subject == null)
                return null;

            return MapToDto(subject);
        }

        // Get subjects belonging to a user
        public IEnumerable<SubjectDto> GetSubjectsByUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be positive.");

            var subjects = _subjectRepository.GetByUserId(userId);

            return subjects.Select(MapToDto).ToList();
        }

        // Add a new subject
        public async System.Threading.Tasks.Task AddSubjectAsync(SubjectDto subjectDto)
        {
            if (subjectDto == null)
                throw new ArgumentNullException(nameof(subjectDto));

            if (string.IsNullOrWhiteSpace(subjectDto.Name))
                throw new ArgumentException("Subject name cannot be empty.");

            var subject = new Subject
            {
                Name = subjectDto.Name,
                Description = subjectDto.Description,
                Difficulty = subjectDto.Difficulty,
                Progress = subjectDto.Progress,
                ExamDate = subjectDto.ExamDate,
                UserId = subjectDto.UserId,
            };

            await _subjectRepository.AddAsync(subject);
            await _subjectRepository.SaveChangesAsync();
            //try
            //{
            //    await _subjectRepository.AddAsync(subject);
            //    await _subjectRepository.SaveChangesAsync();
            //}
            //catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            //{
            //    var error = ex.InnerException?.Message ?? ex.Message;
            //    throw new Exception(error, ex);
            //}
        }
        
        // Update an existing subject
        public void UpdateSubject(SubjectDto subjectDto)
        {
            if (subjectDto == null)
                throw new ArgumentNullException(nameof(subjectDto));

            if (string.IsNullOrWhiteSpace(subjectDto.Name))
                throw new ArgumentException("Subject name cannot be empty.");

            var subject = _subjectRepository.GetById(subjectDto.Id);

            if (subject == null)
                throw new ArgumentException(
                    $"Subject with ID {subjectDto.Id} not found.");

            subject.Name = subjectDto.Name;
            subject.Description = subjectDto.Description;
            subject.Difficulty = subjectDto.Difficulty;
            subject.Progress = subjectDto.Progress;
            subject.ExamDate = subjectDto.ExamDate;
            subject.UserId = subjectDto.UserId;

            _subjectRepository.Update(subject);
            _subjectRepository.SaveChanges();
        }

        // Delete a subject
        public void DeleteSubject(int subjectId)
        {
            var subject = _subjectRepository.GetById(subjectId);

            if (subject == null)
                throw new ArgumentException(
                    $"Subject with ID {subjectId} not found.");

            _subjectRepository.Delete(subject);
            _subjectRepository.SaveChanges();
        }

        // Get subjects with Hard difficulty
        public IEnumerable<SubjectDto> GetHardSubjects()
        {
            var subjects = _subjectRepository.GetAll()
                .Where(s => s.Difficulty == Difficulty.Hard);

            return subjects.Select(MapToDto).ToList();
        }

        // Get the first Hard subject
        public SubjectDto? GetFirstHardSubject()
        {
            var subject = _subjectRepository.GetAll()
                .FirstOrDefault(s => s.Difficulty == Difficulty.Hard);

            return subject == null ? null : MapToDto(subject);
        }

        // Check whether any Hard subjects exist
        public bool HasHardSubjects()
        {
            return _subjectRepository.GetAll()
                .Any(s => s.Difficulty == Difficulty.Hard);
        }

        // Find a subject by name
        public SubjectDto? GetSubjectDtoByName(string subjectName)
        {
            var subject = _subjectRepository.GetAll()
                .FirstOrDefault(s => s.Name == subjectName);

            return subject == null ? null : MapToDto(subject);
        }

        
    }
}
