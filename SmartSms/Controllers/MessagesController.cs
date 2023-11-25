using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSms.Data;
using SmartSms.Model;
using SmartSms.Model.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly SmartSmsDbContext _smartSmsDbContext ;

        public MessageController(SmartSmsDbContext smartSmsDbContext)
        {
            _smartSmsDbContext = smartSmsDbContext;
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


        [HttpGet("{conversationId}")]
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
            var newMessage = new Message()
            {
                ConversationID = request.ConversationId,
                MessageId = new Guid(),
                MessageContent= request.MessageContent,
                OrderInTheList= conversationMessagesCount + 1,
                IsReceived= true,
                CreatedAt = DateTime.Now
            };
            _smartSmsDbContext.Messages.Add(newMessage);
            await _smartSmsDbContext.SaveChangesAsync();
            return Ok(newMessage);
        }
    }
}
