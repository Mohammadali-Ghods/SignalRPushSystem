using MongoDB.Entities;
using System.Collections.Generic;

namespace SignalRWithWebAPIAndMongoDB.Entities
{
    public class Friend : Entity
    {
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string SignalRContextID { get; set; }
        public Status UserStatus { get; set; }
        public List<Friend> MyFriendsList { get; set; }
    }
}
