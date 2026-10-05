using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Task = SmartStudyPlanner.Domain.Entities.Task;

namespace SmartStudyPlanner.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    { 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
       
public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<Task> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure relationships and constraints here if needed
            // For example, if you have a one-to-many relationship between User and StudySession:
            modelBuilder.Entity<User>()
    .HasMany(u => u.StudySessions)
    .WithOne(s => s.User)
    .HasForeignKey(s => s.UserId)
    .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Subjects)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Availabilities)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.StudyPlans)
                .WithOne(sp => sp.User)
                .HasForeignKey(sp => sp.UserId);
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Tasks)
                .WithOne(t => t.Subject)
                .HasForeignKey(t => t.SubjectId);
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.StudySessions)
                .WithOne(ss => ss.Subject)
                .HasForeignKey(ss => ss.SubjectId);
        }
    }
}
