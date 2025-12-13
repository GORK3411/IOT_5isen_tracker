using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5isen_tracker_dll.Data
{
    public class MyApplicationDbContext : DbContext
    {
        public MyApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<WaterContainer> WaterContainers { get; set; }
    }
}
