using _5isen_tracker_dll.Models;
using Microsoft.AspNetCore.Identity;

namespace _5isen_tracker_dll.Repositories.Interfaces;

public interface IUserRepos
{
    Task<IdentityUser?> GetByIdAsync(string id);
    Task<IdentityUser?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<IdentityUser> AddAsync(IdentityUser user);
}
