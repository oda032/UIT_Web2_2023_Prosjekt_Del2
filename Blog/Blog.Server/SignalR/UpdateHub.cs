using Microsoft.AspNetCore.SignalR;

namespace Blog.Server.SignalR
{
    public class UpdateHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
