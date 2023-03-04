using System.ComponentModel.DataAnnotations;

namespace SimpleQueue.WebUI.Models.DataTransferObjects
{
    public class CreateQueueDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string? Password { get; set; }
        public bool IsChat { get; set; }
        public DateTime CreatedTime => DateTime.Now;
        public DateTime? StartTime { get; set; }
        public bool IsDelayed { get; set; }
        public DateTime? DelayedTimeFrom { get; set; }
        public DateTime? DelayedTimeTo { get; set; }
        public int? DurationPerParticipant { get; set; }
        public IList<string> TagsDto { get; set; } = new List<string>();
    }
}
