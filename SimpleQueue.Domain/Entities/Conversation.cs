namespace SimpleQueue.Domain.Entities
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public Guid UserFirstId { get; set; }
        public User UserFirst { get; set; }
        public Guid UserSecondId { get; set; }
        public User UserSecond { get; set; }
        public DateTime Time { get; set; }
    }
}
