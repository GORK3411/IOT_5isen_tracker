using _5isen_tracker_dll.Models;

namespace _5isen_tracker_dll.Repositories.Interfaces;

public interface ILogRepos
{
    Task<Log> AddAsync(Log log);
    Task<Log?> GetLatestForContainerAsync(int waterContainerId);
    Task<List<Log>> GetForContainerAsync(int waterContainerId, int take = 100);
    Task<List<Log>> GetForContainerBetweenAsync(int waterContainerId, DateTime from, DateTime to);
}
