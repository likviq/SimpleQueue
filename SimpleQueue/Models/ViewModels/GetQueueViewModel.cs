namespace SimpleQueue.WebUI.Models.ViewModels
{
    public class GetQueueViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Chat { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime StartTime { get; set; }
        public IEnumerable<UserInQueueViewModel> Users { get; set; }
        = Array.Empty<UserInQueueViewModel>();
    }
}
