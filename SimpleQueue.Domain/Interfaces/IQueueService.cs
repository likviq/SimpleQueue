using SimpleQueue.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Domain.Interfaces
{
    public interface IQueueService
    {
        void CreateQueue(Queue queue);
    }
}
