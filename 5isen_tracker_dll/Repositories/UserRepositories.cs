using Microsoft.EntityFrameworkCore;
using _5isen_tracker_dll.Data;
using _5isen_tracker_dll.Models;
using _5isen_tracker_dll.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace _5isen_tracker_dll.Repositories;

public class UserRepositories : IUserRepos
{
    private readonly MyApplicationDbContext _db;

    public UserRepositories(MyApplicationDbContext db)
    {
        _db = db;
    }


    public Task<IdentityUser?> GetByIdAsync(string id)
        => _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

    public Task<IdentityUser?> GetByEmailAsync(string email)
        => _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

    public Task<bool> ExistsByEmailAsync(string email)
        => _db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email);

    public async Task<IdentityUser> AddAsync(IdentityUser user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
}
