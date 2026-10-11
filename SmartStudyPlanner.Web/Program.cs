using Microsoft.EntityFrameworkCore;
using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Application.Services;
using SmartStudyPlanner.Domain.Entities;
using SmartStudyPlanner.Infrastructure.Data;
using SmartStudyPlanner.Infrastructure.Repositories;

namespace SmartStudyPlanner.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(
     options => options.UseSqlServer(connectionString));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<IStudyPlanningService, StudyPlanningService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();

            var app = builder.Build();

            // Apply migrations and seed data on startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
                SeedData(db);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Subjects}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }

        private static void SeedData(ApplicationDbContext db)
        {
            if (db.Users.Any())
            {
                return;
            }

            var user = new User
            {
                Name = "Ziad Youssef",
                Email = "ziad@example.com",
                PasswordHash = UserService.HashPassword("Password123!"),
                AcademicLevel = "3rd Year",
                ProfilePictureUrl = ""
            };
            db.Users.Add(user);
            db.SaveChanges();

            var today = DateTime.Today;

            db.Subjects.AddRange(
                new Subject
                {
                    Name = "C#",
                    Description = "Object-oriented programming, LINQ and .NET fundamentals.",
                    Difficulty = 85,
                    Progress = 45,
                    ExamDate = today.AddDays(10),
                    Color = "#3b82f6",
                    EstimatedMinutes = 90,
                    UserId = user.Id
                },
                new Subject
                {
                    Name = "SQL",
                    Description = "Joins, functions, indexing and query optimization.",
                    Difficulty = 55,
                    Progress = 70,
                    ExamDate = today.AddDays(5),
                    Color = "#f97316",
                    EstimatedMinutes = 45,
                    UserId = user.Id
                },
                new Subject
                {
                    Name = "Math",
                    Description = "Algebra, calculus and integration techniques.",
                    Difficulty = 90,
                    Progress = 20,
                    ExamDate = today.AddDays(3),
                    Color = "#8b5cf6",
                    EstimatedMinutes = 60,
                    UserId = user.Id
                },
                new Subject
                {
                    Name = "Communication",
                    Description = "Technical writing and presentation skills.",
                    Difficulty = 25,
                    Progress = 60,
                    ExamDate = today.AddDays(12),
                    Color = "#10b981",
                    EstimatedMinutes = 30,
                    UserId = user.Id
                },
                new Subject
                {
                    Name = "Physics",
                    Description = "Mechanics, waves and modern physics.",
                    Difficulty = 60,
                    Progress = 35,
                    ExamDate = today.AddDays(15),
                    Color = "#06b6d4",
                    EstimatedMinutes = 60,
                    UserId = user.Id
                }
            );

            db.SaveChanges();

            var subjects = db.Subjects.Where(s => s.UserId == user.Id).ToList();
            var csharp = subjects.FirstOrDefault(s => s.Name == "C#");
            var sql = subjects.FirstOrDefault(s => s.Name == "SQL");
            var math = subjects.FirstOrDefault(s => s.Name == "Math");

            db.Tasks.AddRange(
                new Domain.Entities.Task
                {
                    Title = "Finish C# LINQ exercises",
                    SubjectId = csharp!.Id,
                    UserId = user.Id,
                    Priority = Domain.Enums.Priority.High,
                    EstimatedMinutes = 90,
                    DueDate = today.AddDays(1),
                    Status = Domain.Enums.TaskStatus.InProgress,
                    IsCompleted = false
                },
                new Domain.Entities.Task
                {
                    Title = "Read SQL joins chapter",
                    SubjectId = sql!.Id,
                    UserId = user.Id,
                    Priority = Domain.Enums.Priority.Medium,
                    EstimatedMinutes = 45,
                    DueDate = today,
                    Status = Domain.Enums.TaskStatus.Pending,
                    IsCompleted = false
                },
                new Domain.Entities.Task
                {
                    Title = "Solve math practice problems",
                    SubjectId = math!.Id,
                    UserId = user.Id,
                    Priority = Domain.Enums.Priority.High,
                    EstimatedMinutes = 60,
                    DueDate = today.AddDays(2),
                    Status = Domain.Enums.TaskStatus.Pending,
                    IsCompleted = false
                },
                new Domain.Entities.Task
                {
                    Title = "Review OOP concepts",
                    SubjectId = csharp.Id,
                    UserId = user.Id,
                    Priority = Domain.Enums.Priority.Medium,
                    EstimatedMinutes = 30,
                    DueDate = today.AddDays(-1),
                    Status = Domain.Enums.TaskStatus.Completed,
                    IsCompleted = true
                }
            );

            db.SaveChanges();
        }
    }
}
