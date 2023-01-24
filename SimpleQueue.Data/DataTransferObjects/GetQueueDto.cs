using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Data.DataTransferObjects
{
    public class GetQueueDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Chat { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime StartTime { get; set; }
        public IEnumerable<UserInQueueDto> Users { get; set; }
        = Array.Empty<UserInQueueDto>();
    }
}
