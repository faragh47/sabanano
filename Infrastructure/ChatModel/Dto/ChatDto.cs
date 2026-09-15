using AutoMapper;
using CleanArchitecture.Application.Common.GlobalDtos;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Infrastructure.ChatModel;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.MessageingModels.Dto;
using Common.Utilities;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.DataTransferObjects.NotificationDTOs;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.ChatingModels.Dto
{
    public class ChatCuDto : BaseDto<ChatCuDto, Chat, long>, IValidatableObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long ContextId { get; set; }
        public long ReferedUserId { get; set; }
        public long CreatedBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //var date = PersianDateExtensions.ToGregorianDate(Date-Time);

            if (CreatedBy <= 0)
                yield return new ValidationResult("وارد کردن فرستنده الزامیست");

            if (ReferedUserId <= 0)
                yield return new ValidationResult("وارد کردن گیرنده الزامیست");
        }
    }
    public class ChatListDto : BaseDto<ChatListDto, Chat, long>
    {
        public ApplicationUserBriefistDto ReferedUser { get; set; }
        public long? CreatedBy { get; set; }
        public List<MessageListDto> Messages { get; set; }
        public override void CustomMappings(IMappingExpression<Chat, ChatListDto> mappingExpression)
        {
            mappingExpression
                .ForMember(dest => dest.ReferedUser, conf => conf.MapFrom(src => src.ReferedUser))
                .ForMember(dest => dest.Messages, conf => conf.MapFrom(src => src.Messages.Take(10)))
              //  .ForMember(dest => dest.Messages, conf => conf.MapFrom(src =>src.Messages))
                .ForMember(dest => dest.CreatedBy, conf => conf.MapFrom(src => src.CreatedBy));
        }
    }
    public class ChatSearchDto : BaseSearchDto//, IHaveCustomExpression<Chat, ChatSearchDto, long>
    {
        public long? ReferedUserId { get; set; }
        public long? CreatedBy { get; set; }

        public Expression<Func<Chat, bool>> GenerateExpression(ChatSearchDto dto)
        {
            List<Expression<Func<Chat, bool>>> expressions = new List<Expression<Func<Chat, bool>>>(); //ExpressionsHelper.GenerateActorsExpression<Notification, NotificationSearchDto, long>(dto);

            if (ReferedUserId is not null)
                expressions.Add(src => src.ReferedUserId == dto.ReferedUserId);

            if (CreatedBy is not null)
                expressions.Add(src => src.CreatedBy == dto.CreatedBy);

            return ExpressionsHelper.AndAll(expressions);
        }
    }
}
