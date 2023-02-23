namespace SimpleQueue.WebUI.Models.ViewModels
{
    public abstract class BriefQueueViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsStarted { get; set; }
        public bool IsFrozen { get; set; }
    }
}
