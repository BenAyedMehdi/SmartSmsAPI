namespace SmartSms.Model.Requests
{
    public class AddMessageRequest
    {
        public Guid ConversationId { get; set; }
        public string MessageContent { get; set; }
    }
}
