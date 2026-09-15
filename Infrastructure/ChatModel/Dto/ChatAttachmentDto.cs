using AutoMapper;
using CleanArchitecture.Application.Common.GlobalDtos;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Infrastructure.ChatModel;
using CleanArchitecture.Infrastructure.Identity;
using Common.Utilities;
using DataTransferObjects.CustomExpressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CleanArchitecture.Infrastructure.ChatingModels.Dto
{
    public class ChatAttachmentCuDto : BaseDto<ChatAttachmentCuDto, ChatAttachment, long>, IValidatableObject
    {
        public long MessageId { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public long SizeInBytes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //var date = PersianDateExtensions.ToGregorianDate(Date-Time);

            if (string.IsNullOrEmpty(FileName))
                yield return new ValidationResult("وارد کردن نام فایل الزامیست");
        }
        public override void CustomMappings(IMappingExpression<ChatAttachmentCuDto, ChatAttachment> mappingExpression)
        {
            mappingExpression.ForMember(dest => dest.MessageId,
                conf => conf.MapFrom(src => src.MessageId));

            mappingExpression.ForMember(dest => dest.FileName,
                conf => conf.MapFrom(src => src.FileName));

            mappingExpression.ForMember(dest => dest.FileExt,
                conf => conf.MapFrom(src => src.FileExt));

            mappingExpression.ForMember(dest => dest.SizeInBytes,
                conf => conf.MapFrom(src => src.SizeInBytes));
        }
    }
    public class ChatAttachmentListDto : BaseDto<ChatAttachmentListDto, ChatAttachment, long>
    {
        public string FileUrl { get; set; }
        public string FileExt { get; set; }
        public long SizeInBytes { get; set; }

        public override void CustomMappings(IMappingExpression<ChatAttachment, ChatAttachmentListDto> mappingExpression)
        {
            mappingExpression
                .ForMember(dest => dest.FileUrl, conf => conf.MapFrom(src => src.FileName))
                .ForMember(dest => dest.FileExt, conf => conf.MapFrom(src => src.FileExt))
                .ForMember(dest => dest.SizeInBytes, conf => conf.MapFrom(src => src.SizeInBytes));

            mappingExpression.ForMember(dest => dest.FileUrl,
                conf => conf.MapFrom(src => $"{Config.ChatAttachmentsPath}/{src.FileName}"));

            mappingExpression.ForMember(dest => dest.FileExt,
                conf => conf.MapFrom(src => src.FileExt));

            mappingExpression.ForMember(dest => dest.SizeInBytes,
                conf => conf.MapFrom(src => src.SizeInBytes));

        }
    }
    public class ChatAttachmentSearchDto : BaseSearchDto//, IHaveCustomExpression<ChatAttachment, ChatAttachmentSearchDto, long>
    {

        //public Expression<Func<ChatAttachment, long>> GenerateExpression(ChatAttachmentSearchDto dto)
        //{
        //    List<Expression<Func<ChatAttachment, long>>> expressions = new List<Expression<Func<ChatAttachment, long>>>();


        //    return ExpressionsHelper.AndAll(expressions);
        //}
    }
}
