namespace SimpleQueue.Domain.Entities
{
    public class Queue
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string? Password { get; set; }
        public bool Chat { get; set; }
        public bool IsFrozen { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime StartTime { get; set; }
        public Guid OwnerId { get; set; }
        public List<UserInQueue> UserInQueues { get; set; } = new List<UserInQueue>();
        public List<QueueTag> QueueTags { get; set; } = new List<QueueTag>();
    }
}
