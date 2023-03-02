using SimpleQueue.Domain.Entities;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Services
{
    public class UserInQueueService : IUserInQueueService
    {
        private readonly IRepositoryManager _repository;
        public UserInQueueService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public Task<UserInQueue?> GetUserInQueueAsync(Guid userInQueueId)
        {
            return _repository.UserInQueue.GetAsync(userInQueueId);
        }

        public void Delete(UserInQueue userInQueue)
        {
            if (userInQueue.DestinationTime == null)
            {
                _repository.UserInQueue.DeleteUserInQueue(userInQueue);
            }
            else
            {
                userInQueue.UserId = null;
            }

            _repository.Save();
        }

        public void EnterQueue(UserInQueue userInQueue)
        {
            _repository.UserInQueue.CreateUserInQueue(userInQueue);
            _repository.Save();
        }

        public Task<bool> IsUserInQueueAsync(Guid userId, Guid queueId)
        {
            return _repository.UserInQueue.IsUserInQueueAsync(userId, queueId);
        }

        public Task<bool> IsDestinationInQueueAsync(Guid queueId, Guid userInQueueId)
        {
            return _repository.UserInQueue.IsDestinationInQueueAsync(queueId, userInQueueId);
        }

        public async Task<int?> UserPositionInQueueAsync(Guid userId, Guid queueId)
        {
            var participant = _repository.UserInQueue.FirstParticipantAsync(queueId).Result;
            var position = 1;

            if (participant == null)
            {
                return null;
            }

            while (participant.UserId != userId && participant.NextId != null)
            {
                participant = await _repository.UserInQueue.GetAsync(participant.NextId);
                position++;
            }

            if (participant.NextId == null && participant.UserId != userId)
            {
                return null;
            }

            return position;
        }

        public async Task<UserInQueue> InitializeUserInQueueAsync(Guid userId, Guid queueId)
        {
            var newParticipant = new UserInQueue { Id = Guid.NewGuid(), QueueId = queueId, UserId = userId };

            var lastParticipantInQueue = await _repository.UserInQueue.LastParticipantInQueueAsync(queueId);

            _repository.UserInQueue.CreateUserInQueue(newParticipant);
            _repository.Save();
           
            if (lastParticipantInQueue == null)
            {
                return newParticipant;
            }

            newParticipant.PreviousId = lastParticipantInQueue.Id;
            lastParticipantInQueue.NextId = newParticipant.Id;
            
            _repository.Save();

            return newParticipant;
        }

        public async Task<UserInQueue> SetUserWithDestinationAsync(Guid userId, Guid userInQueueId)
        {
            var userInQueue = await GetUserInQueueAsync(userInQueueId);
            userInQueue.UserId = userId;

            _repository.Save();

            return userInQueue;
        }

        public void MoveUserInQueueAfter(UserInQueue userInQueue, UserInQueue targetUserInQueue)
        {
            var nextAfterSwapUserInQueue = targetUserInQueue.Next;
            if (nextAfterSwapUserInQueue != null)
            {
                nextAfterSwapUserInQueue.PreviousId = userInQueue.Id;
            }

            var previousBeforeSwapUserInQueue = userInQueue.Previous;
            var nextBeforeSwapUserInQueue = userInQueue.Next;
            if (previousBeforeSwapUserInQueue != null && nextBeforeSwapUserInQueue != null)
            {
                previousBeforeSwapUserInQueue.NextId = nextBeforeSwapUserInQueue.Id;
                nextBeforeSwapUserInQueue.PreviousId = previousBeforeSwapUserInQueue.Id;
            }

            userInQueue.NextId = targetUserInQueue.NextId;
            targetUserInQueue.NextId = userInQueue.Id;
            userInQueue.PreviousId = targetUserInQueue.Id;

            _repository.Save();
        }

        public List<UserInQueue> CreateDelayedPlaces(
            DateTime from, DateTime to, int durationInMinutes)
        {
            var delayedPlaces = new List<UserInQueue>();

            while(from < to)
            {
                var delayedPlace = new UserInQueue()
                {
                    DestinationTime = from
                };
                delayedPlaces.Add(delayedPlace);

                from = from.AddMinutes(durationInMinutes);
            }

            return delayedPlaces;
        }
    }
}
