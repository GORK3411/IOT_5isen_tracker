namespace _5isen_tracker_web_app.Models.Ui;

public class WaterContainerIndexModel
{
    public List<WaterContainerCardModel> Containers { get; set; } = new();

    public int Total => Containers.Count;
    public decimal AvgPercent => Total == 0 ? 0 : Math.Round(Containers.Average(x => x.Percent), 0);
    public decimal MinPercent => Total == 0 ? 0 : Containers.Min(x => x.Percent);
}

public class WaterContainerCardModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string QrCode { get; set; } = "";

    public decimal Percent { get; set; }
    public decimal Liters { get; set; }

    public decimal? LastDistanceCm { get; set; }
    public DateTime? LastUpdateUtc { get; set; }

    // UI sugar (for colors + labels)
    public string StatusText { get; set; } = "—";
    public string StatusClass { get; set; } = "status-neutral";
    public string BarClass { get; set; } = "bar-neutral";
}
