using Microsoft.EntityFrameworkCore;
using _5isen_tracker_dll.Data;
using _5isen_tracker_dll.Models;
using _5isen_tracker_dll.Repositories.Interfaces;

namespace _5isen_tracker_dll.Repositories;

public class LogRepositories : ILogRepos
{
    private readonly MyApplicationDbContext _db;

    public LogRepositories(MyApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Log> AddAsync(Log log)
    {
        _db.Logs.Add(log);
        await _db.SaveChangesAsync();
        return log;
    }

    public Task<Log?> GetLatestForContainerAsync(int deviceId)
        => _db.Logs
            .AsNoTracking()
            .Where(l => l.DeviceId == deviceId)
            .OrderByDescending(l => l.CreatedAt)
            .FirstOrDefaultAsync();

    public Task<List<Log>> GetForContainerAsync(int deviceId, int take = 100)
        => _db.Logs
            .AsNoTracking()
            .Where(l => l.DeviceId == deviceId)
            .OrderByDescending(l => l.CreatedAt)
            .Take(take)
            .ToListAsync();

            public Task<List<Log>> GetForContainerBetweenAsync(int deviceId, DateTime from, DateTime to)
    => _db.Logs
        .AsNoTracking()
        .Where(l => l.DeviceId == deviceId &&
                    l.CreatedAt >= from &&
                    l.CreatedAt <= to)
        .OrderBy(l => l.CreatedAt) // ASC for history curve
        .ToListAsync();

}
