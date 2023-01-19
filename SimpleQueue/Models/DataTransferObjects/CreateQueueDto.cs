namespace SimpleQueue.WebUI.Models.DataTransferObjects
{
    public class CreateQueueDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
    }
}
