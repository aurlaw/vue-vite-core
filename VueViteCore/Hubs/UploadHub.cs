using Microsoft.AspNetCore.SignalR;

namespace VueViteCore.Hubs;

public class UploadHub : Hub<IUploadHubClient>
{

    public async Task Initialize(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }
}
