using SmartStudyPlanner.Application.DTOs;
using SmartStudyPlanner.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace SmartStudyPlanner.Application.Services
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectDto>> GetAllSubjects();

        Task<SubjectDto?> GetSubjectByIdAsync(int subjectId);

        IEnumerable<SubjectDto> GetSubjectsByUserId(int userId);

        Task AddSubjectAsync(SubjectDto subjectDto);

        void UpdateSubject(SubjectDto subjectDto);

        void DeleteSubject(int subjectId);

        SubjectDto? GetSubjectDtoByName(string subjectName);

        IEnumerable<SubjectDto> GetHardSubjects();

        SubjectDto? GetFirstHardSubject();

        bool HasHardSubjects();
    }
}