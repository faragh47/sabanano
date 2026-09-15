//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CleanArchitecture.Infrastructure.ChatModel.Services;
//using CleanArchitecture.Infrastructure.Hubs.IHubs;
//using CleanArchitecture.Infrastructure.MessageingModels.Dto;
//using Common;
//using DataTransferObjects.DataTransferObjects.NotificationDTOs;
//using DataTransferObjects.SharedModels;
//using Microsoft.AspNetCore.SignalR;
//using Services.Hubs.IHubs;

//namespace CleanArchitecture.Infrastructure.Hubs;
//public class ChatHub : Hub<IChatHub>
//{
//    private readonly IChatService _chatService;
//    public List<long> Users { get; set; }
//    public List<long> Supporters { get; set; }

//    public ChatHub(IChatService chatService)
//    {
//        _chatService = chatService;
//    }
//    public async override Task OnConnectedAsync()
//    {
//        var userId = Context.User.Identity.GetUserId();

//        if (Context.User.IsInRole("Supporter"))
//            Supporters.Add(userId);
//        Users.Add(userId);

//        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, userId.ToString());
//    }

//    public async override Task OnDisconnectedAsync(Exception exception)
//    {
//        var userId = Context.User.Identity.GetUserId();

//        if (Context.User.IsInRole("Supporter"))
//        {
//            var exist = Supporters.Any(x => x == userId);
//            if (exist is true)
//                Supporters.Remove(userId);
//        }

//        var existUser = Users.Any(x => x == userId);
//        if (existUser is true)
//            Users.Remove(userId);

//        await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, userId.ToString());
//    }

//    public async Task SendMessage(MessageListDto records)
//    {
//        await this.Clients.All.SendMessages(records);
//    }


//    public async Task<List<long>> GetUsers()
//    {
//        return Users;
//    }
//    public async Task<List<long>> GetSupporters()
//    {
//        return Supporters;
//    }

//}
