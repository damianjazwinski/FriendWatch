using Microsoft.AspNetCore.SignalR;

namespace FriendWatch.Utils
{
    public class CommentsHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", "Test payload");
        }
    }
}
