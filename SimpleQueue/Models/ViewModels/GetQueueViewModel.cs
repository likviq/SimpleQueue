namespace SimpleQueue.WebUI.Models.ViewModels
{
    public class GetQueueViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Chat { get; set; }
        public bool isFrozen { get; set; }
        public int? YourPosition { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? YourId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime StartTime { get; set; }
        public ICollection<UserInQueueViewModel>? Users { get; set; }
    }
}
