namespace SimpleQueue.Domain.Entities
{
    public class QueueType
    {
        public Guid Id { get; set; }
        public TypeName Name { get; set; }
        public List<Queue> Queues { get; set; }
    }

    public enum TypeName
    {
        Fast,
        Delayed
    }
}
