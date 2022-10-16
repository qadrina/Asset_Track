using Microsoft.AspNetCore.SignalR;
using Asset_Track.Shared.StoredProcedures;
using Asset_Track.Shared.Models;


namespace Asset_Track.Server.Hubs
{
    public class AssetHub : Hub<IChatClient>
    {
        public async Task SendEntityToGroup(string sender, string receiver, spAsset _spAsset, int increment)
        {
            await Clients.Group(receiver).ReceiveEntity(sender, receiver, Context.ConnectionId, _spAsset, increment);
        }

        public async Task AddToGroup(string receiver)
        {
            if (!String.IsNullOrEmpty(receiver))
                await Groups.AddToGroupAsync(Context.ConnectionId, receiver);
        }

        public async Task RemoveFromGroup(string receiver)
        {
            if (!String.IsNullOrEmpty(receiver))
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, receiver);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.ReceiveMessage("🟩Sender>", "🟩Receiver", Context.ConnectionId, $"OnConnected {Context.ConnectionId}");
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            Console.WriteLine($"<OnDisconnected> => {Context.ConnectionId}");
            await base.OnDisconnectedAsync(ex);
        }

    }

    public interface IChatClient
    {
        Task ReceiveEntity(string sender, string receiver, string connectionId, spAsset _spAsset, int increment);
        Task ReceiveMessage(string sender, string receiver, string connectionId, string message);
    }
}
