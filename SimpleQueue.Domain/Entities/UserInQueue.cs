namespace SimpleQueue.Domain.Entities
{
    public class UserInQueue
    {
        public Guid Id { get; set; }
        public Guid QueueId { get; set; }
        public Queue Queue { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime JoinTime { get; set; }
        public Guid? NextId { get; set; }
        public UserInQueue? Next { get; set; }
        public Guid? PreviousId { get; set; }
        public UserInQueue? Previous { get; set; }
    }
}
