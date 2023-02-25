using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string TagTitle { get; set; }
        public List<QueueTag> QueueTags { get; set; } = new List<QueueTag>();
    }
}
