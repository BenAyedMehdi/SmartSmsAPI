using SmartSms.Data;
using SmartSms.Model;
using SmartSms.Model.Requests;
using SmartSms.Service.Interfaces;

namespace SmartSms.Service
{
    public class MessagingService : IMessagingService
    {
        private readonly SmartSmsDbContext _smartSmsDbContext;
        private readonly IGptService _gptService;

        public MessagingService(SmartSmsDbContext smartSmsDbContext, IGptService gptService)
        {
            _smartSmsDbContext = smartSmsDbContext;
            _gptService = gptService;
        }

        public async Task<Message> SendMessageAndGetTheAnswer(AddMessageRequest request, int count)
        {
            var newMessage = new Message()
            {
                ConversationID = request.ConversationId,
                MessageId = new Guid(),
                MessageContent = request.MessageContent,
                OrderInTheList = count + 1,
                IsReceived = true,
                CreatedAt = DateTime.Now
            };
            _smartSmsDbContext.Messages.Add(newMessage);
            await _smartSmsDbContext.SaveChangesAsync();

            var generatedReply = await _gptService.GenerateTextAsync(request.MessageContent);
            generatedReply= generatedReply + "\n\n-Intelligence powered by your telecom provieder-";

            var newAnswerMessage = new Message()
            {
                ConversationID = request.ConversationId,
                MessageId = new Guid(),
                MessageContent = generatedReply,
                OrderInTheList = count + 2,
                IsReceived = false,
                CreatedAt = DateTime.Now
            };
            _smartSmsDbContext.Messages.Add(newAnswerMessage);
            await _smartSmsDbContext.SaveChangesAsync();


            return newAnswerMessage;
        }
    }
}
