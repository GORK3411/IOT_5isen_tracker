using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5isen_tracker_dll.Models
{
    public class Device
    {
        public int Id { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 16)]
        public string NodeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // when device is create the datetime will be the current datetime

        public ICollection<Log> Logs { get; set; } = new List<Log>();
        public WaterContainer WaterContainer { get; set; } 
    }

}
