using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
using SmartStudyPlanner.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStudyPlanner.Infrastructure.Repositories
{
    public class StudyPlanRepository : Repository<StudyPlan>, IStudyPlanRepository
    {
        public StudyPlanRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StudyPlan>> GetAllStudyPlansWithSessionsAsync()
        {
            return await _context.StudyPlans.Include(sp => sp.StudySessions).ToListAsync();
        }

        public async Task<IEnumerable<StudyPlan>> GetStudyPlansByUserIdAsync(int userId)
        {
            return await _context.StudyPlans.Where(sp => sp.UserId == userId).Include(sp => sp.StudySessions).ToListAsync();
        }

        public async Task<StudyPlan>  GetStudyPlanWithSessionsAsync(int studyPlanId)
        {
            // possible null reference exception if the study plan with the given id does not exist, consider handling that case : 
            if (!await _context.StudyPlans.AnyAsync(sp => sp.Id == studyPlanId))
            {
                throw new KeyNotFoundException($"Study plan with id {studyPlanId} not found.");
            }
            if ( _context.StudyPlans == null)
            {
                throw new InvalidOperationException("StudyPlans DbSet is null.");
            }
            var studyPlan = await _context.StudyPlans.Include(sp => sp.StudySessions).FirstOrDefaultAsync(sp => sp.Id == studyPlanId);
            return studyPlan!;

        }
    }
}
