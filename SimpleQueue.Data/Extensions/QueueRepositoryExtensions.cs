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

        public static IQueryable<Queue> FilterQueuesByFrozen(
            this IQueryable<Queue> queues, bool? IsFrozen)
        {
            if (IsFrozen is null)
            {
                return queues;
            }

            return queues.Where(queue => queue.IsFrozen.Equals(IsFrozen));
        }

        public static IQueryable<Queue> FilterQueuesByChat(
            this IQueryable<Queue> queues, bool? IsChat)
        {
            if (IsChat is null)
            {
                return queues;
            }

            return queues.Where(queue => queue.Chat.Equals(IsChat));
        }

        public static IQueryable<Queue> FilterQueuesByPassword(
            this IQueryable<Queue> queues, bool? IsPassword)
        {
            if (IsPassword is null)
            {
                return queues;
            }

            return queues.Where(queue => string.IsNullOrWhiteSpace(queue.Password).Equals(!IsPassword));
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

        public static IQueryable<Queue> SortBy(
            this IQueryable<Queue> queues, int? sortByParameter)
        {
            switch (sortByParameter)
            {
                case 0: return queues;
                case 1: return queues.OrderByDescending(q => q.CreatedTime);
                case 2: return queues.OrderBy(q => q.CreatedTime);
                case 3: return queues.OrderByDescending(q => q.UserInQueues.Count);
                case 4: return queues.OrderBy(q => q.UserInQueues.Count);
            }

            return queues;
        }
    }
}
