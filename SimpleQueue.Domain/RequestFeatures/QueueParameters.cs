using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Domain.RequestFeatures
{
    public class QueueParameters: RequestParameters
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; } = DateTime.MaxValue;
        public bool ValidTimeRange => StartTime <= EndTime;

        public string SearchTerm { get; set; }
    }
}
