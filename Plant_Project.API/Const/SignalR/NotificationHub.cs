using Microsoft.AspNetCore.SignalR;

namespace Plant_Project.API.Const.SignalR
{
    public class NotificationHub:Hub
    {
        public static Dictionary<string, string> userConnections = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                userConnections[userId] = Context.ConnectionId;
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                userConnections.Remove(userId);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
