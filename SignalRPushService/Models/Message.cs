namespace SignalRPushService.Models
{
    public class Message
    {
        public string Value { get; set; }
        public string ToSessionID { get; set; }
        public string Channel { get; set; }
    }
}
