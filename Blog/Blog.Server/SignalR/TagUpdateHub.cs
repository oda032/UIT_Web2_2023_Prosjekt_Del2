using Microsoft.AspNetCore.SignalR;

namespace Blog.Server.SignalR
{
    public class TagUpdateHub : Hub
    {
        public async Task SendMessageToUser(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceivePrivateMessage", message);
        }
    }
}
