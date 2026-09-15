using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataTransferObjects.DataTransferObjects.NotificationDTOs;
using DataTransferObjects.SharedModels;
using Microsoft.AspNetCore.SignalR;
using Services.Hubs.IHubs;

namespace Services.Hubs
{
    public class TicketHub : Hub<ITicketHub>
    {
        public async override Task OnConnectedAsync()
        {

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, Context.User.Identity.GetUserId().ToString());
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, Context.User.Identity.GetUserId().ToString());
        }

        public async Task SendMessage(ApiResult<NotificationListDto> records)
        {
            await this.Clients.All.SendNotification(records);
        }

    }

  
}
