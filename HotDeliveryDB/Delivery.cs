using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotDeliveryDB
{
    public class Delivery
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public int? UserId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ModificationTime { get; set; }
        public int ExpirationTime { get; set; }
    }
}
