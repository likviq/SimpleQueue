using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQueue.Domain.Models
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public Guid UserFirstId { get; set; }
        public User UserFirst { get; set; }
        public Guid UserSecondId { get; set; }
        public User UserSecond { get; set; }
        public DateTime Time { get; set; }
    }
}
