namespace SimpleQueue.WebUI.Models.ViewModels
{
    public class BriefQueueInfoViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool isStarted { get; set; }
        public bool isFrozen { get; set; }
    }
}
