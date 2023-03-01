namespace SimpleQueue.WebApi.Models.ViewModels
{
    public class QueueSearchResultViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageLink { get; set; }
        public bool IsFrozen { get; set; }
    }
}
