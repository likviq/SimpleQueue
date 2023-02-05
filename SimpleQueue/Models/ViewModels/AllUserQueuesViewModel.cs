using SimpleQueue.Domain.Entities;

namespace SimpleQueue.WebUI.Models.ViewModels
{
    public class AllUserQueuesViewModel
    {
        public AllUserQueuesViewModel(List<BriefQueueInfoViewModel> _ownerQueues, List<BriefQueueInfoViewModel> _participantQueues)
        {
            ownerQueues = _ownerQueues;
            participantQueues = _participantQueues;
        }
        public List<BriefQueueInfoViewModel> ownerQueues { get; set; }
        public List<BriefQueueInfoViewModel> participantQueues { get; set; }
    }
}
