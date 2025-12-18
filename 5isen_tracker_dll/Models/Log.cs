using System;

namespace _5isen_tracker_dll.Models
{
    public class Log
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }
        public Device Device { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public decimal DistanceCm { get; set; } // distance entre capteur et eau
    }
}
