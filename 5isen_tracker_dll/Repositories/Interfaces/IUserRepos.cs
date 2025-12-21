using _5isen_tracker_dll.Models;

namespace _5isen_tracker_dll.Repositories.Interfaces;

public interface IUserRepos
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<User> AddAsync(User user);
}
