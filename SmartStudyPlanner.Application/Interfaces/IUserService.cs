using SmartStudyPlanner.Domain.Entities;

namespace SmartStudyPlanner.Application.Interfaces
{
    public interface IUserService
    {
        User? GetUserByEmail(string email);
        Task<User?> GetUserById(int id);
        Task<User> Add(User user);
        Task<User> Update(User user);
        bool CheckPassword(User user, string password);
    }
}
