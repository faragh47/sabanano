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
    public class TicketStatusCuDto : BaseDto<TicketStatusCuDto, TicketStatus, int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public override void CustomMappings(IMappingExpression<TicketStatusCuDto, TicketStatus> mappingExpression)
        {
            //mappingExpression.ForMember(dest => dest.entityField,
            //        conf => conf.MapFrom(src => src.dtofield));
        }
    }

    public class TicketStatusListDto : BaseDto<TicketStatusListDto, TicketStatus, int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public override void CustomMappings(IMappingExpression<TicketStatus, TicketStatusListDto> mappingExpression)
        {
            //mappingExpression.ForMember(dest => dest.dtoField,
            //        conf => conf.MapFrom(src => src.entityfield));
        }
    }

    public class TicketStatusSearchDto : BaseSearchDto//, IHaveCustomExpression<TicketStatus, TicketStatusSearchDto, int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public Expression<Func<TicketStatus, bool>> GenerateExpression(TicketStatusSearchDto dto)
        {
            List<Expression<Func<TicketStatus, bool>>> expressions = new List<Expression<Func<TicketStatus, bool>>>();//ExpressionsHelper.GenerateActorsExpression<TicketStatus, TicketStatusSearchDto, int>(dto);

            if (!string.IsNullOrEmpty(dto.Title))
                expressions.Add(src => src.Title.Contains(dto.Title));
            //if (dto.Xid.HasValue)
            //    if (dto.XId > 0)
            //        expressions.Add(src => src.FXId == dto.XId);
            return ExpressionsHelper.AndAll(expressions);
        }
    }
}
