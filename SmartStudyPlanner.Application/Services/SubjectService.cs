
using SmartStudyPlanner.Application.DTOs;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStudyPlanner.Application.Services
{
    public class SubjectService : ISubjectService
    {
        // subjects at or above this difficulty percentage are considered "hard"
        private const int HardDifficultyThreshold = 70;

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
                Color = string.IsNullOrWhiteSpace(subject.Color) ? "#F97316" : subject.Color,
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
                Color = string.IsNullOrWhiteSpace(subjectDto.Color) ? "#F97316" : subjectDto.Color,
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
            subject.Color = string.IsNullOrWhiteSpace(subjectDto.Color) ? "#F97316" : subjectDto.Color;
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

        // Get subjects with high difficulty (>= 70%)
        public IEnumerable<SubjectDto> GetHardSubjects()
        {
            var subjects = _subjectRepository.GetAll()
                .Where(s => s.Difficulty >= HardDifficultyThreshold);

            return subjects.Select(MapToDto).ToList();
        }

        // Get the hardest subject (highest difficulty percentage)
        public SubjectDto? GetFirstHardSubject()
        {
            var subject = _subjectRepository.GetAll()
                .Where(s => s.Difficulty >= HardDifficultyThreshold)
                .OrderByDescending(s => s.Difficulty)
                .FirstOrDefault();

            return subject == null ? null : MapToDto(subject);
        }

        // Check whether any hard subjects exist
        public bool HasHardSubjects()
        {
            return _subjectRepository.GetAll()
                .Any(s => s.Difficulty >= HardDifficultyThreshold);
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
