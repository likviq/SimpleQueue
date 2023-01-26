namespace SimpleQueue.WebUI.Models.ViewModels
{
    public class UserInQueueViewModel
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
