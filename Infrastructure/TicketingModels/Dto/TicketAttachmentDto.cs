using AutoMapper;
using CleanArchitecture.Application.Common.GlobalDtos;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Infrastructure.Identity;
using Common.Utilities;
using DataTransferObjects.CustomExpressions;
using Entities.DatabaseModels.TicketingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.TicketingModels.Dto
{
    public class TicketAttachmentCuDto : BaseDto<TicketAttachmentCuDto, TicketAttachment, long>, IValidatableObject
    {
        public long TicketResponseId { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public long SizeInBytes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //var date = PersianDateExtensions.ToGregorianDate(Date-Time);

            if (string.IsNullOrEmpty(FileName))
                yield return new ValidationResult("وارد کردن نام فایل الزامیست");
        }
        public override void CustomMappings(IMappingExpression<TicketAttachmentCuDto, TicketAttachment> mappingExpression)
        {
            mappingExpression.ForMember(dest => dest.TicketResponseId,
                conf => conf.MapFrom(src => src.TicketResponseId));

            mappingExpression.ForMember(dest => dest.FileName,
                conf => conf.MapFrom(src => src.FileName));

            mappingExpression.ForMember(dest => dest.FileExt,
                conf => conf.MapFrom(src => src.FileExt));

            mappingExpression.ForMember(dest => dest.SizeInBytes,
                conf => conf.MapFrom(src => src.SizeInBytes));
        }
    }
    public class TicketAttachmentListDto : BaseDto<TicketAttachmentListDto, TicketAttachment, long>
    {
        public string FileUrl { get; set; }
        public string FileExt { get; set; }
        public long SizeInBytes { get; set; }

        public override void CustomMappings(IMappingExpression<TicketAttachment, TicketAttachmentListDto> mappingExpression)
        {
            mappingExpression
                .ForMember(dest => dest.FileUrl, conf => conf.MapFrom(src => src.FileName))
                .ForMember(dest => dest.FileExt, conf => conf.MapFrom(src => src.FileExt))
                .ForMember(dest => dest.SizeInBytes, conf => conf.MapFrom(src => src.SizeInBytes));

            mappingExpression.ForMember(dest => dest.FileUrl,
                conf => conf.MapFrom(src => $"{Config.TicketAttachmentsPath}/{src.FileName}"));

            mappingExpression.ForMember(dest => dest.FileExt,
                conf => conf.MapFrom(src => src.FileExt));

            mappingExpression.ForMember(dest => dest.SizeInBytes,
                conf => conf.MapFrom(src => src.SizeInBytes));

        }
    }
    public class TicketAttachmentSearchDto : BaseSearchDto//, IHaveCustomExpression<TicketAttachment, TicketAttachmentSearchDto, long>
    {

        //public Expression<Func<TicketAttachment, long>> GenerateExpression(TicketAttachmentSearchDto dto)
        //{
        //    List<Expression<Func<TicketAttachment, long>>> expressions = new List<Expression<Func<TicketAttachment, long>>>();


        //    return ExpressionsHelper.AndAll(expressions);
        //}
    }
}
