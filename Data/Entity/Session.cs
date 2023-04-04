using MongoDB.Entities;
using System;

namespace Data.Models
{
    public class Session : Entity
    {
        public string IP { get; set; }
        public string FireBaseToken { get; set; }
        public string SignalRToken { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SwtToken { get; set; }
        public string UserID { get; set; }
    }
}
