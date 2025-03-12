using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TaskIcosoftBackend.Hubs
{
    public class TaskHub : Hub
    {
      public async Task NotifyTaskChange(int taskId)
        {
            await Clients.All.SendAsync("TaskUpdated", taskId);
        } 
    }
}