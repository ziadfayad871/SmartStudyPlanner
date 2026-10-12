using SmartStudyPlanner.Application.Interfaces;
using SmartStudyPlanner.Domain.Entities;
namespace SmartStudyPlanner.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }
        public async Task<User?> GetUserById(int id)
        {
            if (id == 0)
            {
                throw new InvalidOperationException("User not found");
            }
            var user = await _userRepository.GetByIdAsync(id);

            return user;
        }
        public async Task<User> Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentNullException(nameof(user.Email));
            }
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                throw new ArgumentNullException(nameof(user.PasswordHash));
            }
            if (string.IsNullOrEmpty(user.Name))
            {
                throw new ArgumentNullException(nameof(user.Name));
            }
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }
        // Update an existing user (image, name, email, password, academic level)
        public async Task<User> Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }
        public async System.Threading.Tasks.Task Delete(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new ArgumentException("User not found.", nameof(id));
            }
            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
        }
        // check password hash for a user by BCrypt.Net
        public bool CheckPassword(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
