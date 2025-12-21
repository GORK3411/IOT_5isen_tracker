using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _5isen_tracker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushSubscriptionController : ControllerBase
    {
        /*
        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] PushSubscriptionDto dto)
        {
            var sub = new PushSubscription
            {
                UserId = User.GetUserId(),
                Endpoint = dto.endpoint,
                P256dh = dto.keys.p256dh,
                Auth = dto.keys.auth,
                CreatedAt = DateTime.UtcNow
            };

            _db.PushSubscriptions.Add(sub);
            await _db.SaveChangesAsync();

            return Ok();
        }
        */
    }
}
