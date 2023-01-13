using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleQueue.Domain.Models
{
    public class UserInQueue
    {
        [Column("UserInQueueId")]
        public Guid Id { get; set; }
        public Guid QueueId { get; set; }
        public Queue Queue { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime JoinTime { get; set; }
        public Guid? NextId { get; set; }
        public UserInQueue? Next { get; set; }
    }
}
