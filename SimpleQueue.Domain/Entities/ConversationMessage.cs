namespace SimpleQueue.Domain.Entities
{
    public class ConversationMessage
    {
        public Guid Id { get; set; }
        public string ContentText { get; set; }
        public Guid SenderUserId { get; set; }
        public User SenderUser { get; set; }
        public DateTime SendTime { get; set; }
        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; }
    }
}
