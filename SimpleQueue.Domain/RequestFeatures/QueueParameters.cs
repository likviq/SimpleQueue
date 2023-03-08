namespace SimpleQueue.Domain.RequestFeatures
{
    public class QueueParameters: RequestParameters
    {
        /// <summary>
        /// The datetime after which queues should be searched, must be less, than EndTime. Minimum datetime value is default
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// The datetime before which queues should be searched. Datetime.Now is default
        /// </summary>
        public DateTime EndTime { get; set; } = DateTime.Now;
        public bool ValidTimeRange => StartTime <= EndTime;
        /// <summary>
        /// The value to find the queues by
        /// </summary>
        public string SearchTerm { get; set; }
        /// <summary>
        /// false - All unfrozen queues, true - All Frozen queues
        /// </summary>
        public bool? IsFrozen { get; set; }
        /// <summary>
        /// false - there is not a chat in the queue, true - there is a chat in the queue
        /// </summary>
        public bool? IsChat { get; set; }
        /// <summary>
        /// false - without password, true - with password
        /// </summary>
        public bool? IsProtected { get; set; }
        /// <summary>
        /// 0 - Default, 1 - The newest queues, 2 - The oldest queues, 3 - The most popular queues, 4 - The least popular queues
        /// </summary>
        public int? SortBy { get; set; }
    }

    public enum Frozen
    {
        Frozen = 1,
        Unfrozen = 0
    }

    public enum Chat
    {
        Chat = 1,
        NoChat = 0
    }

    public enum Privacy
    {
        Password = 1,
        NoPassword = 0
    }

    public enum SortBy
    {
        Recommended = 0,
        Newest = 1,
        Oldest = 2,
        Popularity = 3,
        Insignificance = 4
    }
}
