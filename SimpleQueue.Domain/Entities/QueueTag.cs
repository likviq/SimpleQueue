using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Domain.Entities
{
    public class QueueTag
    {
        public Guid Id { get; set; }
        public Queue? Queue { get; set; }
        public Guid QueueId { get; set; }
        public Tag? Tag { get; set; }
        public Guid TagId { get; set; }
    }
}
