using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Application.Services
{
    public interface ISubjectService 
    {
        IEnumerable<Subject>  GetAllSubjects();
        Subject ? GetSubjectById(int subjectId);
        IEnumerable<Subject> GetSubjectsByUserId(int userId);
        void AddSubject(Subject subject);
        void UpdateSubject(Subject subject);
        void DeleteSubject(int subjectId);

    }
}
