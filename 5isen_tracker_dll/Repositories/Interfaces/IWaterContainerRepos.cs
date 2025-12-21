using _5isen_tracker_dll.Models;

namespace _5isen_tracker_dll.Repositories.Interfaces;

public interface IWaterContainer
{
    Task<WaterContainer?> GetByIdAsync(int id);
    Task<WaterContainer?> GetByQrCodeAsync(string qrCode);
    Task<List<WaterContainer>> GetByUserIdAsync(string userId);

    Task<WaterContainer> AddAsync(WaterContainer container);
    Task UpdateAsync(WaterContainer container);
    Task DeleteAsync(int id);
}
