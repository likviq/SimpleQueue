using SimpleQueue.Domain.Entities;
using System;
using System.Collections.Generic;

public class FakeDataGenerator
{
    public static IEnumerable<object[]> GetQueueFromDataGenerator()
    {
        yield return new object[]
        {
            new Queue
            {
                Id = Guid.Parse("f2bd3543-ebbc-4b41-a4ed-5be0c87c5c2e"),
                Title = "Frozen queue",
                Description = "Description",
                Chat = true,
                IsFrozen = true,
            }
        };

        yield return new object[]
        {
            new Queue
            {
                Id = Guid.Parse("920cc596-e573-4cfd-89f8-94ac18290d10"),
                Title = "Not frozen queue",
                Description = "Description",
                Chat = true,
                IsFrozen = false,
            }
        };
    }

    public static IEnumerable<object[]> GetUserIdAndQueueIdFromDataGenerator()
    {
        yield return new object[] { Guid.Empty, Guid.Empty };

        yield return new object[] { Guid.Empty, Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e") };
    }

    public static IEnumerable<object[]> InitializeUserInQueueAsyncDataGenerator()
    {
        yield return new object[] { Guid.Empty, Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e") };

        yield return new object[] { Guid.Empty, Guid.Parse("920cc596-e573-4cfd-89f8-94ac18290d10") };
    }
}