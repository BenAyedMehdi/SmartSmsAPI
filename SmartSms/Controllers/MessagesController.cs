using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSms.Data;
using SmartSms.Model;
using SmartSms.Model.Requests;
using SmartSms.Service;
using SmartSms.Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly SmartSmsDbContext _smartSmsDbContext;
        private readonly IGptService _gptService;
        private readonly IMessagingService _messagingService;

        public MessageController(SmartSmsDbContext smartSmsDbContext, IGptService gptService, IMessagingService messagingService)
        {
            _smartSmsDbContext = smartSmsDbContext;
            _gptService = gptService;
            _messagingService = messagingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> Get()
        {
            var allMessages = await _smartSmsDbContext
              .Messages
              .ToListAsync();
            return Ok(allMessages);
        }

        [HttpGet("{messageId}")]
        public async Task<ActionResult<Message>> GetById(Guid messageId)
        {
            var foundMessage = await _smartSmsDbContext
               .Messages
               .FirstOrDefaultAsync(m => m.MessageId == messageId);
            if (foundMessage == null)
            {
                return NotFound();
            }
            return Ok(foundMessage);
        }


        [HttpGet("conversation/{conversationId}")]
        public async Task<ActionResult<IEnumerable<Conversation>>> GetAllByConversationId(Guid conversationId)
        {
            var foundConversationMessages = await _smartSmsDbContext
               .Messages
               .Where( m => m.ConversationID == conversationId)
               .ToListAsync();
            if (foundConversationMessages == null)
            {
                return NotFound();
            }
            return Ok(foundConversationMessages);
        }

        [HttpPost]
        public async Task<ActionResult<Conversation>> Post(AddMessageRequest request)
        {
            var allConversationMessages = await GetAllByConversationId(request.ConversationId);
            var conversationMessagesCount = 0;
            if (allConversationMessages.Value != null)
            {
                conversationMessagesCount=  allConversationMessages.Value.ToList().Count;
            }

            var replySMS = await _messagingService.SendMessageAndGetTheAnswer(request, conversationMessagesCount);
            
            return Ok(replySMS);
        }
    }
}
