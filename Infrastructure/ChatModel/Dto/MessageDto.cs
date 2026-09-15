using AutoMapper;
using CleanArchitecture.Application.Common.GlobalDtos;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Infrastructure.Identity;
using Common.Utilities;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.ChatModel;
using CleanArchitecture.Infrastructure.ChatingModels.Dto;


namespace CleanArchitecture.Infrastructure.MessageingModels.Dto
{

    public class MessageWithAttachmentCuDto : BaseDto<MessageWithAttachmentCuDto, Message, long>, IValidatableObject
    {
        public long ChatId { get; set; }
        public string Description { get; set; }
        public long? InReplyToResponseId { get; set; }

        public List<ChatAttachmentCuDto> Attachments { get; set; }

        public override void CustomMappings(IMappingExpression<Message, MessageWithAttachmentCuDto> mappingExpression)
        {
            mappingExpression
                .ForMember(dest => dest.Attachments, conf => conf.MapFrom(src => src.Attachments));
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //var date = PersianDateExtensions.ToGregorianDate(Date-Time);
            if (ChatId <= 0)
                yield return new ValidationResult("وارد کردن شناسه چت الزامیست");

        }
    }

    public class MessageCuDto : BaseDto<MessageCuDto, Message, long>, IValidatableObject
    {
        public long ChatId { get; set; }
        public string Description { get; set; }
        public long? InReplyToResponseId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //var date = PersianDateExtensions.ToGregorianDate(Date-Time);
            if (ChatId <= 0)
                yield return new ValidationResult("وارد کردن شناسه چت الزامیست");

        }
    }
    public class MessageListDto : BaseDto<MessageListDto, Message, long>
    {
        public long ChatId { get; set; }
        public string Description { get; set; }
        public long? InReplyToResponseId { get; set; }
        public List<ChatAttachmentListDto> Attachments { get; set; }

        public override void CustomMappings(IMappingExpression<Message, MessageListDto> mappingExpression)
        {
            mappingExpression
                .ForMember(dest => dest.Attachments, conf => conf.MapFrom(src => src.Attachments));
        }

    }
    public class MessageSearchDto : BaseSearchDto//, IHaveCustomExpression<Message, MessageSearchDto, long>
    {

        //public Expression<Func<Message, long>> GenerateExpression(MessageSearchDto dto)
        //{
        //    List<Expression<Func<Message, long>>> expressions = new List<Expression<Func<Message, long>>>();


        //    return ExpressionsHelper.AndAll(expressions);
        //}
    }
}
