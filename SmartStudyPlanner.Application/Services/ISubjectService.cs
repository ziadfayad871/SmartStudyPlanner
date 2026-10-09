using SmartStudyPlanner.Application.DTOs;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
namespace SmartStudyPlanner.Application.Services
{
    public interface ISubjectService 
    {
        Task<IEnumerable<Subject>>  GetAllSubjects();
        Subject ? GetSubjectById(int subjectId);
        IEnumerable<Subject> GetSubjectsByUserId(int userId);
        Task AddSubjectAsync(Subject subject);
       Task<Subject?> GetSubjectByIdAsync(int subjectId);
        SubjectDto? GetSubjectDtoById(int subjectId);
        IEnumerable<Subject> GetHardSubjects();
        SubjectDto? GetSubjectDtoByName(string subjectName);
        Subject? GetFirstHardSubject();
        bool HasHardSubjects();

    }
}
