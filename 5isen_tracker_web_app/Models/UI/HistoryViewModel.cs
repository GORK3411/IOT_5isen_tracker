namespace _5isen_tracker_web_app.Models.Ui
{
    public class HistoryViewModel
    {
        public int ContainerId { get; set; }
        public string ContainerName { get; set; } = "";

        // Chart arrays
        public List<string> Labels { get; set; } = new();
        public List<decimal> Percents { get; set; } = new();
        public List<decimal> Liters { get; set; } = new();

        // Optional: show last update
        public DateTime? LastUpdateUtc { get; set; }
    }
}
