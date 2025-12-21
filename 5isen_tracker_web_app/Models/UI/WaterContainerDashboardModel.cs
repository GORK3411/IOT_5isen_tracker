
using System.Collections.Generic;

namespace _5isen_tracker_web_app.Models.Ui;

public class WaterContainerDashboardModel
{
    public List<WaterContainerCardModel> Containers { get; set; } = new();

    public int Total => Containers.Count;
    public decimal AvgPercent => Total == 0 ? 0 : Math.Round(Containers.Average(x => x.Percent), 0);
    public decimal MinPercent => Total == 0 ? 0 : Containers.Min(x => x.Percent);
}
