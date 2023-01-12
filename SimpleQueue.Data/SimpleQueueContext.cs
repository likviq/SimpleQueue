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
    }
}
