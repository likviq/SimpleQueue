using Moq;
using SimpleQueue.Domain.Interfaces;

namespace SimpleQueue.Services.Tests.Mocks
{
    internal class MockRepositoryManager
    {
        public static Mock<IRepositoryManager> GetMock()
        {
            var mock = new Mock<IRepositoryManager>();
            var queueRepoMock = MockIQueueRepository.GetMock();

            mock.Setup(m => m.Queue).Returns(() => queueRepoMock.Object);
            mock.Setup(m => m.Save()).Callback(() => { return; });

            return mock;
        }
    }
    
}
