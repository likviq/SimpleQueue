using Microsoft.EntityFrameworkCore;
using SimpleQueue.Domain.Models;

namespace SimpleQueue.Data
{
    public class SimpleQueueDBContext : DbContext
    {
        public SimpleQueueDBContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<Queue> Queues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserInQueue> UserInQueues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInQueue>()
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.UserInQueues)
                    .HasForeignKey(pt => pt.UserId);
            
            modelBuilder.Entity<UserInQueue>()
                    .HasOne(pt => pt.Queue)
                    .WithMany(t => t.UserInQueues)
                    .HasForeignKey(pt => pt.QueueId);

            modelBuilder.Entity<UserInQueue>()
                    .Property(pt => pt.JoinTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            modelBuilder.Entity<UserInQueue>()
                    .Property(pt => pt.NextId).HasDefaultValue(null);
            

        }
    }
}
