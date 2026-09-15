using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using Common.Utilities;
using DataTransferObjects.CustomExpressions;
using Entities.DatabaseModels.TicketingModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.TicketingModels.Dto
{
    public class TicketUserResponseCuDto : BaseDto<TicketUserResponseCuDto, TicketUserResponse, long>, IValidatableObject
    {
        public long TicketId { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? InReplyToResponseId { get; set; }
        public List<IFormFile> Files { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //var date = PersianDateExtensions.ToGregorianDate(Date-Time);

            if (string.IsNullOrEmpty(Description))
                yield return new ValidationResult("وارد کردن پاسخ الزامیست");
        }
        public override void CustomMappings(IMappingExpression<TicketUserResponseCuDto, TicketUserResponse> mappingExpression)
        {
            mappingExpression
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description));
        }
    }
    public class TicketUserResponseListDto : BaseDto<TicketUserResponseListDto, TicketUserResponse, long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long TicketId { get; set; }
        public string Response { get; set; }
        public long? InReplyToResponseId { get; set; }
        public long CreatedBy { get; set; }

        public ICollection<TicketUserResponseListDto> Responses { get; set; }
        public ICollection<TicketAttachmentListDto> TicketAttachments { get; set; }

        public override void CustomMappings(IMappingExpression<TicketUserResponse, TicketUserResponseListDto> mappingExpression)
        {
            mappingExpression
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description))
                .ForMember(dest => dest.Response, conf => conf.MapFrom(src => src.Description))
                .ForMember(dest => dest.Responses, conf => conf.MapFrom(src => src.Children))
                .ForMember(dest => dest.CreatedBy, conf => conf.MapFrom(src => src.CreatedBy));
        }
    }
    public class TicketUserResponseSearchDto : BaseSearchDto//, IHaveCustomExpression<TicketUserResponse, TicketUserResponseSearchDto, long>
    {
        public int? Id { get; set; }

        public Expression<Func<TicketUserResponse, bool>> GenerateExpression(TicketUserResponseSearchDto dto)
        {
            List<Expression<Func<TicketUserResponse, bool>>> expressions = new List<Expression<Func<TicketUserResponse, bool>>>();
            if (Id is not null)
                if (dto.Id > 0)
                    expressions.Add(src => src.Id == dto.Id);

            return ExpressionsHelper.AndAll(expressions);
        }
    }
}
