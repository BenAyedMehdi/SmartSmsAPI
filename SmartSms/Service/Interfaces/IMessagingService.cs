using SmartSms.Model;
using SmartSms.Model.Requests;

namespace SmartSms.Service.Interfaces
{
    public interface IMessagingService
    {
        Task<Message> SendMessageAndGetTheAnswer(AddMessageRequest request, int count);
    }
}
