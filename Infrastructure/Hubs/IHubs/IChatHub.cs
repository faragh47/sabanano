using System;
using CleanArchitecture.Infrastructure.MessageingModels.Dto;
using DataTransferObjects.DataTransferObjects.NotificationDTOs;
using DataTransferObjects.SharedModels;

namespace CleanArchitecture.Infrastructure.Hubs.IHubs
{
    public interface IChatHub
    {
        Task SendMessages(MessageListDto recrod);
        Task<List<long>> GetUsers();
        Task<List<long>> GetSupporters();
    }
}

