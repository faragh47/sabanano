using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.CustomMapping;
using Entities.DatabaseModels.TicketingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.TicketingModels.Dto
{
    public class TicketReferCuDto : BaseDto<TicketReferCuDto, TicketReferHistory, long>
    {
        public long TicketId { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public override void CustomMappings(IMappingExpression<TicketReferCuDto, TicketReferHistory> mappingExpression)
        {
        }
    }

    public class TicketReferListDto : BaseDto<TicketReferListDto, TicketReferHistory, long>
    {
        public long TicketId { get; set; }
        public long FromUserId { get; set; }
        public string FromUserFullName { get; set; }
        public string ToUserId { get; set; }
        public string ToUserFullName { get; set; }
        public string CreatorFullName { get; set; }
        public string CreationDate_TimeStamp { get; set; }
        public override void CustomMappings(IMappingExpression<TicketReferHistory, TicketReferListDto> mappingExpression)
        {
            mappingExpression.ForMember(dest => dest.FromUserFullName,
                    conf => conf.MapFrom(src => src.FromUser.FullName));
            mappingExpression.ForMember(dest => dest.ToUserFullName,
                    conf => conf.MapFrom(src => src.ToUser.FullName));
            mappingExpression.ForMember(dest => dest.CreatorFullName,
                    conf => conf.MapFrom(src => src.Creator.FullName));
            mappingExpression.ForMember(dest => dest.CreationDate_TimeStamp, conf =>
                    conf.MapFrom(src => new DateTimeOffset((DateTime)src.Created).ToUnixTimeSeconds()));
        }
    }

    public class TicketReferSearchDto : BaseSearchDto//, IHaveCustomExpression<TicketReferHistory, TicketReferSearchDto, long>
    {
        public int? TicketId { get; set; }

        public Expression<Func<TicketReferHistory, bool>> GenerateExpression(TicketReferSearchDto dto)
        {
            List<Expression<Func<TicketReferHistory, bool>>> expressions =new List<Expression<Func<TicketReferHistory, bool>>>() ;//ExpressionsHelper.GenerateActorsExpression<TicketReferHistory, TicketReferSearchDto, long>(dto);

            //if (!String.IsNullOrEmpty(dto.Title))
            //    expressions.Add(src => src.Title.Contains(dto.Title));
            if (dto.TicketId.HasValue)
                if (dto.TicketId > 0)
                    expressions.Add(src => src.TicketId == dto.TicketId);

            return ExpressionsHelper.AndAll(expressions);
        }
    }
}
