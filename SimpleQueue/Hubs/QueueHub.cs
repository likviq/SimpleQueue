using Microsoft.AspNetCore.SignalR;

namespace SimpleQueue.WebUI.Hubs
{
    public class QueueHub: Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("Receive", message);
        }
    }
}
