using SmartStudyPlanner.Domain.Entities;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetUserByEmail(string email);
    }
}
