namespace SimpleQueue.WebUI.Models.ViewModels
{
    public class GetQueueViewModel
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
