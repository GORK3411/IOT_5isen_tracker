using _5isen_tracker_dll.Data;
using _5isen_tracker_dll.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace _5isen_tracker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {

        private readonly MyApplicationDbContext _db;

        public DeviceController(MyApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ICollection<Device> Get()
        {
            return _db.Devices.ToList();
        }


        [HttpPost("AddDevice")]
        public async Task<IActionResult> AddDevice(string nodeId)
        {
            string userId = "fbc7fc0c-b822-451b-a3a7-868193387ca5";
            var user = _db.Users.First(); 

            var devices = _db.Devices;
            if (devices.Any(d => d.NodeId== nodeId ))
            {
                return BadRequest("There is already a device with this nodeId");
            }
            try
            {
                var device = new Device {NodeId = nodeId,User = user};
                devices.Add(device);
                _db.SaveChanges();
                return Ok($"Device {nodeId} have been added");
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}
