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

        public async Task<UserInQueue?> GetUserInQueue(Guid userInQueueId)
        {
            return await _repository.UserInQueue.Get(userInQueueId);
        }

        public void Delete(UserInQueue userInQueue)
        {
            _repository.UserInQueue.DeleteUserInQueue(userInQueue);
            _repository.Save();
        }

        public void EnterQueue(UserInQueue userInQueue)
        {
            _repository.UserInQueue.CreateUserInQueue(userInQueue);
            _repository.Save();
        }

        public bool IsUserInQueue(Guid userId, Guid queueId)
        {
            return _repository.UserInQueue.IsUserInQueue(userId, queueId);
        }

        public async Task<int?> UserPositionInQueue(Guid userId, Guid queueId)
        {
            var participant = _repository.UserInQueue.FirstParticipant(queueId).Result;
            var position = 1;

            if (participant == null)
            {
                return null;
            }

            while (participant.UserId != userId && participant.NextId != null)
            {
                participant = await _repository.UserInQueue.Get(participant.NextId);
                position++;
            }

            if (participant.NextId == null && participant.UserId != userId)
            {
                return null;
            }

            return position;
        }

        public async Task<UserInQueue> InitializeUserInQueue(Guid userId, Guid queueId)
        {
            var newParticipant = new UserInQueue { Id = Guid.NewGuid(), QueueId = queueId, UserId = userId };

            var lastParticipantInQueue = await _repository.UserInQueue.LastParticipantInQueue(queueId);

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
    }
}
