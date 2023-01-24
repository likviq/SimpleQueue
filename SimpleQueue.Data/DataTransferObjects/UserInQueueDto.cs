using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Data.DataTransferObjects
{
    public class UserInQueueDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime JoinTime { get; set; }
        public Guid IdInQueue { get; set; }
        public Guid? NextIdInQueue { get; set; }
    }
}
