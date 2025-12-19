using _5isen_tracker_dll.Data;
using _5isen_tracker_dll.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;

namespace _5isen_tracker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        MyApplicationDbContext _db;
        public LogController(MyApplicationDbContext db) 
        {
            this._db = db;
        }

        [HttpPost]
        public IActionResult Post([FromBody]DeviceLogDto deviceLog)
        {

            
            var device = _db.Devices
                            .FirstOrDefault(d => d.NodeId == deviceLog.NodeId);

            if (deviceLog.Distance<0)
            {
                return BadRequest("Distance can't be negative");
            }
            if (device == null)
            {
               return BadRequest("Aucun device trouvé");
            }

            try
            {
                var logs = _db.Logs;
                var newLog = new Log { DistanceCm = deviceLog.Distance , Device = device};
                logs.Add(newLog);
                _db.SaveChanges();
                return Ok("SAVED");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class DeviceLogDto
        {
            public string NodeId { get; set; }
            public decimal Distance { get; set; }
        }

    }
}
