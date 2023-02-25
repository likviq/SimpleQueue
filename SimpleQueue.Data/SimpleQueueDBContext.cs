using Microsoft.EntityFrameworkCore;
using SimpleQueue.Data.Configurations;
using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Data
{
    public class SimpleQueueDBContext : DbContext
    {
        public SimpleQueueDBContext(DbContextOptions<SimpleQueueDBContext> options) 
            : base(options)
        {
        }

        public DbSet<Queue> Queues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserInQueue> UserInQueues { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationMessage> ConversationMessages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QueueTag> QueueTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserEntityConfiguration().Configure(modelBuilder.Entity<User>());

            new QueueEntityConfiguration().Configure(modelBuilder.Entity<Queue>());

            new UserInQueueEntityConfiguration().Configure(modelBuilder.Entity<UserInQueue>());
            
            new ConversationEntityConfiguration().Configure(modelBuilder.Entity<Conversation>());

            new ConversationMessageEntityConfiguration().Configure(modelBuilder.Entity<ConversationMessage>());

            new TagEntityConfiguration().Configure(modelBuilder.Entity<Tag>());

            new QueueTagEntityConfiguration().Configure(modelBuilder.Entity<QueueTag>());
        }
    }
}
