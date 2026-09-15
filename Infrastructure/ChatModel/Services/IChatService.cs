//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CleanArchitecture.Infrastructure.ChatingModels.Dto;
//using CleanArchitecture.Infrastructure.MessageingModels.Dto;
//using CleanArchitecture.Infrastructure.TicketingModels.Dto;
//using Common;
//using DataTransferObjects.DataTransferObjects.UserDTOs;
//using DataTransferObjects.SharedModels;
//using Microsoft.AspNetCore.Http;

//namespace CleanArchitecture.Infrastructure.ChatModel.Services;
//public interface IChatService : IScopedDependency
//{
//    Task<ApiResult<ChatListDto>> StartOperation(ChatSearchDto dto, CancellationToken cancellationToken);
//    Task<ApiResult<List<ApplicationUserBriefistDto>>> GetOnlineSupporters(ChatSearchDto dto, CancellationToken cancellationToken);
//    Task<ApiResult<List<ApplicationUserBriefistDto>>> GetOnlineUsers(ChatSearchDto dto, CancellationToken cancellationToken);
//    Task<ApiResult<MessageListDto>> CreateMessage(MessageCuDto dto);
//    Task<ApiResult<List<MessageListDto>>> GetChatMessages(ChatSearchDto dto, CancellationToken cancellationToken);
//    Task<ApiResult<MessageListDto>> Attachment(MessageWithAttachmentCuDto dto, List<IFormFile> files, long creatorId, string folderPath, CancellationToken cancellationToken);
//}

