﻿using SimpleQueue.Domain.Entities;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IUserInQueueRepository
    {
        Task<UserInQueue?> FirstParticipant(Guid queueId);
        void DeleteUserInQueue(UserInQueue userInQueue);
        Task<UserInQueue?> Get(Guid? userInQueueId);
        void CreateUserInQueue(UserInQueue userInQueue);
        bool IsUserInQueue(Guid userId, Guid queueId);
        bool IsDestinationInQueue(Guid queueId, Guid userInQueueId);
        Task<UserInQueue?> LastParticipantInQueue(Guid queueId);
    }
}
