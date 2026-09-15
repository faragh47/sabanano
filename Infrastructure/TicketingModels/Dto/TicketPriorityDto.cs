using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.CustomMapping;
using Entities.DatabaseModels.TicketingModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataTransferObjects.DataTransferObjects.TicketingDTOs
{
    public class TicketPriorityCuDto : BaseDto<TicketPriorityCuDto, TicketPriority, int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public int ResponseTime { get; set; }

        public override void CustomMappings(IMappingExpression<TicketPriorityCuDto, TicketPriority> mappingExpression)
        {
            //mappingExpression.ForMember(dest => dest.entityField,
            //        conf => conf.MapFrom(src => src.dtofield));
        }
    }

    public class TicketPriorityBriefListDto : BaseDto<TicketPriorityBriefListDto, TicketPriority, int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public int ResponseTime { get; set; }
    }


    public class TicketPriorityListDto : BaseDto<TicketPriorityListDto, TicketPriority, int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public int ResponseTime { get; set; }

        public override void CustomMappings(IMappingExpression<TicketPriority, TicketPriorityListDto> mappingExpression)
        {
            //mappingExpression.ForMember(dest => dest.dtoField,
            //        conf => conf.MapFrom(src => src.entityfield));
            // mappingExpression.AddActorMappings<TicketPriority, TicketPriorityListDto, int>();
        }
    }

    public class TicketPrioritySearchDto : BaseSearchDto//, IHaveCustomExpression<TicketPriority, TicketPrioritySearchDto, int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public int ResponseTime { get; set; }

        public Expression<Func<TicketPriority, bool>> GenerateExpression(TicketPrioritySearchDto dto)
        {
            List<Expression<Func<TicketPriority, bool>>> expressions = new List<Expression<Func<TicketPriority, bool>>>();
            //ExpressionsHelper.GenerateActorsExpression<TicketPriority, TicketPrioritySearchDto, int>(dto);

            if (!String.IsNullOrEmpty(dto.Title))
                expressions.Add(src => src.Title.Contains(dto.Title));

            if (dto.ResponseTime > 0)
                expressions.Add(src => src.ResponseTime == dto.ResponseTime);

            return ExpressionsHelper.AndAll(expressions);
        }
    }
}
