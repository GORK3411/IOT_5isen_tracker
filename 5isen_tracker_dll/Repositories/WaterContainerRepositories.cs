using Microsoft.EntityFrameworkCore;
using _5isen_tracker_dll.Data;
using _5isen_tracker_dll.Models;
using _5isen_tracker_dll.Repositories.Interfaces;

namespace _5isen_tracker_dll.Repositories;

public class WaterContainerRepositories : IWaterContainer
{
    private readonly MyApplicationDbContext _db;

    public WaterContainerRepositories(MyApplicationDbContext db)
    {
        _db = db;
    }

    public Task<WaterContainer?> GetByIdAsync(int id)
        => _db.WaterContainers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

    public Task<WaterContainer?> GetByQrCodeAsync(string qrCode)
        => _db.WaterContainers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.QrCode == qrCode);

    public Task<List<WaterContainer>> GetByUserIdAsync(string userId)
        => _db.WaterContainers
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Id)
            .ToListAsync();

    public async Task<WaterContainer> AddAsync(WaterContainer container)
    {
        _db.WaterContainers.Add(container);
        await _db.SaveChangesAsync();
        return container;
    }

    public async Task UpdateAsync(WaterContainer container)
    {
        _db.WaterContainers.Update(container);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.WaterContainers
            .FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null)
            return;

        _db.WaterContainers.Remove(entity);
        await _db.SaveChangesAsync();
    }
}
