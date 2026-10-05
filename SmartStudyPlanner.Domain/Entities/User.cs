namespace SmartStudyPlanner.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<StudyPlan> StudyPlans { get; set; } = new List<StudyPlan>();
        public ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        // availability of the user for study sessions
        public ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
    }
}