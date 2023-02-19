using Microsoft.AspNetCore.SignalR;

namespace SimpleQueue.WebUI.Hubs
{
    public class QueueHub: Hub
    {
        public async Task Clicker(string groupName, int number)
        {
            await Clients.Group(groupName).SendAsync("increaseClicker", number);
        }

        public async Task NextUser(string groupName, string username)
        {
            await Clients.Group(groupName).SendAsync("nextUser", username);
        }

        public async Task EnterQueue(string groupName, string queueId, string userInQueueId, string username)
        {
            await Clients.Group(groupName).SendAsync("enterQueue", queueId, userInQueueId, username);
        }

        public async Task LeaveQueue(string groupName, string queueId, string userInQueueId, int userPreviousPosition)
        {
            await Clients.Group(groupName).SendAsync("leaveQueue", queueId, userInQueueId, userPreviousPosition);
        }

        public async Task FreezeQueue(string groupName)
        {
            await Clients.Group(groupName).SendAsync("freezeQueue");
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
