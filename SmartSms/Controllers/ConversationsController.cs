using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSms.Data;
using SmartSms.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly SmartSmsDbContext _smartSmsDbContext ;

        public ConversationsController(SmartSmsDbContext smartSmsDbContext)
        {
            _smartSmsDbContext = smartSmsDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conversation>>> Get()
        {
            var allConversations = await _smartSmsDbContext
              .Conversations
              .Include(c => c.Messages)
              .ToListAsync();
            return Ok(allConversations);
        }

        // GET api/<ConversationController>/5
        [HttpGet("{conversationId}")]
        public async Task<ActionResult<Conversation>> Get(Guid conversationId)
        {
            var foundConversation = await _smartSmsDbContext
               .Conversations
               .Include(c => c.Messages)
               .FirstOrDefaultAsync( c => c.ConversationID == conversationId);
            if (foundConversation == null)
            {
                return NotFound();
            }
            return Ok(foundConversation);
        }

        // POST api/<ConversationController>
        [HttpPost]
        public async Task<ActionResult<Conversation>> Post()//([FromBody] string value)
        {
            var newConversation = new Conversation()
            {
                ConversationID = Guid.NewGuid(),
                Messages = new List<Message>(),
                CreatedAt= DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _smartSmsDbContext.Conversations.Add(newConversation);
            await _smartSmsDbContext.SaveChangesAsync();
            return Ok(newConversation);
        }

    }
}
