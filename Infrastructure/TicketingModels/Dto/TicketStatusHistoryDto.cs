using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.CustomMapping;
using Entities.DatabaseModels.TicketingModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.TicketingModels.Dto
{
    public class TicketStatusHistoryCuDto : BaseDto<TicketStatusHistoryCuDto, TicketStatusHistory, long>
    {
        public long TicketId { get; set; }
        public int StatusId { get; set; }
        public override void CustomMappings(IMappingExpression<TicketStatusHistoryCuDto, TicketStatusHistory> mappingExpression)
        {
          
        }
    }

    public class TicketStatusHistoryListDto : BaseDto<TicketStatusHistoryListDto, TicketStatusHistory, long>
    {
        public long TicketId { get; set; }
        public int StatusId { get; set; }
        public string StatusTitle { get; set; }
        public override void CustomMappings(IMappingExpression<TicketStatusHistory, TicketStatusHistoryListDto> mappingExpression)
        {
      
            mappingExpression.ForMember(dest => dest.StatusTitle,
                    conf => conf.MapFrom(src => src.TicketStatus.Title));

        }
    }

    public class TicketStatusHistorySearchDto : BaseSearchDto//, IHaveCustomExpression<TicketStatusHistory, TicketStatusHistorySearchDto, long>
    {
        public int? TicketId { get; set; }
        public int? StatusId { get; set; }
        public Expression<Func<TicketStatusHistory, bool>> GenerateExpression(TicketStatusHistorySearchDto dto)
        {
            List<Expression<Func<TicketStatusHistory, bool>>> expressions = new List<Expression<Func<TicketStatusHistory, bool>>>();//ExpressionsHelper.GenerateActorsExpression<TicketStatusHistory, TicketStatusHistorySearchDto, long>(dto);

            //if (!String.IsNullOrEmpty(dto.Title))
            //    expressions.Add(src => src.Title.Contains(dto.Title));
            if (TicketId is not null)
                if (dto.TicketId > 0)
                    expressions.Add(src => src.TicketId == dto.TicketId);
            if (StatusId is not null)
                if (dto.StatusId > 0)
                    expressions.Add(src => src.StatusId == dto.StatusId);
            return ExpressionsHelper.AndAll(expressions);
        }
    }
}
