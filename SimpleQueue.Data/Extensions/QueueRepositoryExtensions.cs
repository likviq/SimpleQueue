using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Data.Extensions
{
    public static class QueueRepositoryExtensions
    {
        public static IQueryable<Queue> FilterQueuesByTime(
            this IQueryable<Queue> queues, DateTime StartTime, DateTime EndTime)
        {
            return queues.Where(queue => queue.StartTime >= StartTime && queue.StartTime <= EndTime);
        }

        public static IQueryable<Queue> Search(
            this IQueryable<Queue> queues, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return queues;
            }
            
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return queues.Where(queue => (queue.Title + queue.Description)
                .ToLower().Contains(lowerCaseTerm));
        }
    }
}
