namespace SmartSms.Model
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public int OrderInTheList { get; set; }
        public bool IsReceived { get; set; }
        public string MessageContent { get; set; }
        public DateTime CreatedAt { get; set; }
     
        public Guid ConversationID{ get; set; }
    }
}
