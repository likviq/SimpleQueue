namespace SimpleQueue.WebApi.Models.ViewModels
{
    public class UserInDelayedQueueViewModel
    {
        public string UserInQueueId { get; set; }
        public string UserId { get; set; }
        public string QueueId { get; set; }
        public DateTime DestinationTime { get; set; }

    }
}
