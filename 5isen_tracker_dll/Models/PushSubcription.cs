using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5isen_tracker_dll.Models
{
    public class PushSubscription
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string Endpoint { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}
