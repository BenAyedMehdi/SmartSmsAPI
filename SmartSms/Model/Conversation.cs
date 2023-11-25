namespace SmartSms.Model
{
    public class Conversation
    {
        public Guid ConversationID { get; set; }
        public List<Message> Messages { get; set; }  

        public DateTime CreatedAt { get; set; } 
        public DateTime ModifiedAt { get; set; } 
    }
}
