namespace SimpleQueue.Domain.RequestFeatures
{
    public class QueueParameters: RequestParameters
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; } = DateTime.Now;
        public bool ValidTimeRange => StartTime <= EndTime;
        public string SearchTerm { get; set; }
        public bool? IsFrozen { get; set; }
        public bool? IsChat { get; set; }
        public bool? IsProtected { get; set; }
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
