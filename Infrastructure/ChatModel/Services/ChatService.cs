//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using CleanArchitecture.Infrastructure.ChatingModels.Dto;
//using CleanArchitecture.Infrastructure.Hubs;
//using CleanArchitecture.Infrastructure.Hubs.IHubs;
//using CleanArchitecture.Infrastructure.Identity;
//using CleanArchitecture.Infrastructure.MessageingModels.Dto;
//using CleanArchitecture.Infrastructure.TicketingModels.Dto;
//using Common;
//using Common.Exceptions;
//using Common.Utilities;
//using Data.Contracts;
//using DataTransferObjects.DataTransferObjects.UserDTOs;
//using DataTransferObjects.SharedModels;
//using Entities.DatabaseModels.TicketingModels;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
//using Services.Hubs;

//namespace CleanArchitecture.Infrastructure.ChatModel.Services;
//public class ChatService : IChatService
//{
//    private readonly IRepository<Chat> _repository;
//    private readonly IRepository<Message> _messageRepository;
//    private readonly IRepository<ChatAttachment> _chatAttachmentRepository;
//    private readonly IRepository<ApplicationUser> _userRepository;
//    private readonly IMapper _mapper;
//    private readonly IHubContext<ChatHub, IChatHub> _hubContext;
//    public ChatService(IRepository<Chat> repository,
//                       IMapper mapper, IHubContext<ChatHub,
//                       IChatHub> hubContext, IRepository<ApplicationUser> userRepository,
//                       IRepository<Message> messageRepository, IRepository<ChatAttachment> chatAttachmentRepository)
//    {
//        _repository = repository;
//        _mapper = mapper;
//        _hubContext = hubContext;
//        _userRepository = userRepository;
//        _messageRepository = messageRepository;
//        _chatAttachmentRepository = chatAttachmentRepository;
//    }

//    public async Task<ApiResult<MessageListDto>> Attachment(MessageWithAttachmentCuDto dto, List<IFormFile> files, long creatorId, string folderPath, CancellationToken cancellationToken)
//    {
//        var ticketAttachmentCuDto = new MessageWithAttachmentCuDto
//        {
//            ChatId = dto.ChatId,
//            Description = dto.Description,
//            InReplyToResponseId = dto.InReplyToResponseId,
//        };

//        var entity = ticketAttachmentCuDto.ToEntity(_mapper);

//        await _messageRepository.AddAsync(entity, cancellationToken);

//        for (int i = 0; i < files.Count; i++)
//        {
//            if (files[i].Length == 0)
//                throw new BadRequestException("حجم فایل تصویر صفر است.");

//            var filePath = FileOperationsExtension.GetUniqueFilePath(folderPath, "T" + Path.GetExtension(files[i].FileName));

//            if (String.IsNullOrEmpty(filePath))
//                throw new BadRequestException("مسیر ذخیره فایل درست نیست یا نام آن اشکال دارد.");

//            using (var stream = System.IO.File.Create(filePath))
//            {
//                await files[i].CopyToAsync(stream);
//            }

//            var AttachmentCuDto = new ChatAttachmentCuDto
//            {
//                MessageId = entity.Id,
//                FileName = Path.GetFileName(filePath),
//                FileExt = Path.GetExtension(filePath),
//                SizeInBytes = files[i].Length
//            };

//            var entityAttachment = AttachmentCuDto.ToEntity(_mapper);
//            await _chatAttachmentRepository.AddAsync(entityAttachment, cancellationToken);

//        }

//        var result = await _messageRepository.TableNoTracking
//                  .ProjectTo<MessageListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == entity.Id);

//        return result;

//    }

//    public async Task<ApiResult<MessageListDto>> CreateMessage(MessageCuDto dto)
//    {
//        var entity = dto.ToEntity(_mapper);

//        var chat = await _repository.TableNoTracking.Where(x => x.Id == dto.ChatId)
//               .ProjectTo<ChatListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

//        if (chat is null)
//            return new ApiResult<MessageListDto>(false, ApiResultStatusCode.ChatNotFound, null);

//        await _messageRepository.AddAsync(entity, CancellationToken.None);

//        var result = await _messageRepository.TableNoTracking.ProjectTo<MessageListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == entity.Id);

//        if (result is not null)
//        {
//            await _hubContext.Clients.Group(chat.ReferedUser.Id.ToString()).SendMessages(result);
//        }

//        return result;
//    }

//    public async Task<ApiResult<List<MessageListDto>>> GetChatMessages(ChatSearchDto dto, CancellationToken cancellationToken)
//    {
//        var messages = await _messageRepository.TableNoTracking.Where(x => x.ChatId == dto.Id)
//                        .ProjectTo<MessageListDto>(_mapper.ConfigurationProvider).ToListAsync();

//        return messages;
//    }

//    public async Task<ApiResult<List<ApplicationUserBriefistDto>>> GetOnlineSupporters(ChatSearchDto dto, CancellationToken cancellationToken)
//    {
//        var supporters = await _hubContext.Clients.All.GetSupporters();

//        if (!supporters.Any())
//            return new ApiResult<List<ApplicationUserBriefistDto>>(false, ApiResultStatusCode.NotFound, null);

//        var result = await _userRepository.TableNoTracking.Where(x => supporters.Contains(x.Id)).
//            ProjectTo<ApplicationUserBriefistDto>(_mapper.ConfigurationProvider).ToListAsync();

//        return result;
//    }

//    public async Task<ApiResult<List<ApplicationUserBriefistDto>>> GetOnlineUsers(ChatSearchDto dto, CancellationToken cancellationToken)
//    {
//        var users = await _hubContext.Clients.All.GetUsers();

//        if (!users.Any())
//            return new ApiResult<List<ApplicationUserBriefistDto>>(false, ApiResultStatusCode.NotFound, null);

//        var result = await _userRepository.TableNoTracking.Where(x => users.Contains(x.Id)).
//            ProjectTo<ApplicationUserBriefistDto>(_mapper.ConfigurationProvider).ToListAsync();

//        return result;
//    }

//    public async Task<ApiResult<ChatListDto>> StartOperation(ChatSearchDto dto, CancellationToken cancellationToken)
//    {
//        if (dto.CreatedBy is null && dto.ReferedUserId is null)
//            return new ApiResult<ChatListDto>(false, ApiResultStatusCode.BadRequest, null);

//        var expression = dto.GenerateExpression(dto);

//        var chat = await _repository.TableNoTracking.Where(expression)
//                   .ProjectTo<ChatListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

//        if (chat is null)
//        {
//            var createDto = new ChatCuDto()
//            {
//                CreatedBy = Convert.ToInt64(dto.CreatedBy),
//                ReferedUserId = Convert.ToInt64(dto.ReferedUserId)
//            };

//            var entity = createDto.ToEntity(_mapper);

//            await _repository.AddAsync(entity, cancellationToken);

//            chat = await _repository.TableNoTracking.Where(expression)
//                   .ProjectTo<ChatListDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
//        }

//        return chat;
//    }
//}
