namespace SimpleQueue.Domain.Entities
{
    public class UserInQueue
    {
        public Guid Id { get; set; }
        public Guid QueueId { get; set; }
        public virtual Queue? Queue { get; set; }
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
        public DateTime JoinTime { get; set; }
        public Guid? NextId { get; set; }
        public virtual UserInQueue? Next { get; set; }
    }
}
