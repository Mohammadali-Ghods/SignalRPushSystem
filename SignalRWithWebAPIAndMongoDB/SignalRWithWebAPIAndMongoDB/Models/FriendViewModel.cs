using SignalRWithWebAPIAndMongoDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWithWebAPIAndMongoDB.Models
{
    public class FriendViewModel
    {
        public string ID { get; set; }
        public string FullName { get; set; }
        public Status UserStatus { get; set; }
    }
}
