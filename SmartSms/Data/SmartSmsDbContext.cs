using Microsoft.EntityFrameworkCore;
using SmartSms.Model;

namespace SmartSms.Data

{
    public class SmartSmsDbContext : DbContext
    {
        public SmartSmsDbContext(DbContextOptions<SmartSmsDbContext> options)
            : base(options)
        { }

        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
