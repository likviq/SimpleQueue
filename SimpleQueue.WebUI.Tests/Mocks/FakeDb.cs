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
            Description = "Description",
            Chat = true,
            IsFrozen = false,
        },
        new Queue
        {
            Id = Guid.Parse("920cc596-e573-4cfd-89f8-94ac18290d10"),
            Title = "DelayedQueue",
            Description = "Description",
            Chat = true,
            IsFrozen = false,
        }
    };

    public static List<UserInQueue> UserInQueuesDelayed = new List<UserInQueue>()
    {
        new UserInQueue
        {
            Id = Guid.Parse("d704fb0c-9d12-4b1c-b0a6-9f74d4cd477b"),
            QueueId = Guid.Parse("920cc596-e573-4cfd-89f8-94ac18290d10"),
            DestinationTime = DateTime.Now,
        }
    };

    public static List<QueueType> QueueTypes = new List<QueueType>()
    {
        new QueueType
        {
            Id = Guid.Parse("34779a74-9424-437f-9d2e-ea0bb51ae99d"),
            Name = TypeName.Fast
        },
        new QueueType
        {
            Id = Guid.Parse("3c840c34-3967-4300-bbd9-45844ea9fdb9"),
            Name = TypeName.Delayed
        }
    };

    public static List<Tag> Tags = new List<Tag>()
    {
        new Tag
        {
            Id = Guid.Parse("088bbe03-e962-47cd-b4a9-8453584ad81e"),
            TagTitle = "FirstTag"
        }
    };

    public static List<QueueTag> QueueTags = new List<QueueTag>()
    {
        new QueueTag
        {
            Id = Guid.Parse("b7097127-7585-4263-8118-b78042626527"),
            TagId = Guid.Parse("088bbe03-e962-47cd-b4a9-8453584ad81e")
        }
    };
}