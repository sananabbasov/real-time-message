using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebUI.Hubs;

[Authorize]
public class MessageHub : Hub
{

    public async Task SendMessage(string user, string message)
    {
        var users = Context.GetHttpContext().User;
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
