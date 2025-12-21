namespace _5isen_tracker_dll.Models;

public class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    public ICollection<WaterContainer> WaterContainers { get; set; } = new List<WaterContainer>();
}
