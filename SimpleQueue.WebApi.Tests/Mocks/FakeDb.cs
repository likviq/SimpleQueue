using SimpleQueue.Domain.Entities;
using System;
using System.Collections.Generic;

public static class FakeDb
{
    public static List<Queue> Queues = new List<Queue>()
    {
        new Queue
        {
            Id = Guid.Parse("f2bd3543-ebbc-4b41-a4ed-5be0c87c5c2e"),
            Title = "FastQueue",
            Description = "Description1",
            Chat = true,
            IsFrozen = false,
            OwnerId = Guid.Parse("9355a7df-c059-4c23-8c1f-31ec31962944")
        },
        new Queue
        {
            Id = Guid.Parse("96f4cb8d-4c79-48a0-ae18-74fc1bc10225"),
            Title = "DelayedQueue",
            Description = "Description2",
            Chat = true,
            IsFrozen = false,
            OwnerId = Guid.Parse("9355a7df-c059-4c23-8c1f-31ec31962944")
        }
    };

    public static List<UserInQueue> UserInQueues = new List<UserInQueue>()
    {
        new UserInQueue
        {
            Id = Guid.Parse("9eb451b2-cbc6-4b89-9582-a1a27814e752"),
            QueueId = Guid.Parse("f2bd3543-ebbc-4b41-a4ed-5be0c87c5c2e"),
            UserId = Guid.Parse("cccc3ce5-9a09-4b41-bc16-6c2e13007066")
        },
        new UserInQueue
        {
            Id = Guid.Parse("5850e5c8-eaa2-4fff-95b3-35dafc2ef04c"),
            QueueId = Guid.Parse("96f4cb8d-4c79-48a0-ae18-74fc1bc10225"),
            UserId = Guid.Parse("d76bb0e2-a57b-47e3-b937-50ab501909de")
        },
        new UserInQueue
        {
            Id = Guid.Parse("0c5fbb98-b650-436c-97b2-9505e10d74fa"),
            QueueId = Guid.Parse("96f4cb8d-4c79-48a0-ae18-74fc1bc10225"),
            UserId = Guid.Parse("cccc3ce5-9a09-4b41-bc16-6c2e13007066")
        }
    };

    public static List<User> Users = new List<User>()
    {
        new User
        {
            Id = Guid.Parse("9355a7df-c059-4c23-8c1f-31ec31962944"),
            Username = "owner"
        },
        new User
        {
            Id = Guid.Parse("cccc3ce5-9a09-4b41-bc16-6c2e13007066"),
            Username = "not owner"
        },
        new User
        {
            Id = Guid.Parse("d76bb0e2-a57b-47e3-b937-50ab501909de"),
            Username = "not participant"
        }
    };
}