using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleQueue.Domain.Models
{
    public class Queue
    {
        [Column("QueueId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Queue title is a required field")]
        [StringLength(25, ErrorMessage = "", MinimumLength = 3)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Queue title is a required field")]
        [StringLength(255, ErrorMessage = "", MinimumLength = 3)]
        public string Description { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        [PasswordPropertyText]
        public string? Password { get; set; }
        [DefaultValue(false)]
        public bool Chat { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }
        public List<UserInQueue> UserInQueues { get; set; } = new List<UserInQueue>();
    }
}
