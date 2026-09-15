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
    public class TicketCategoryCuDto : BaseDto<TicketCategoryCuDto, TicketCategory, int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public override void CustomMappings(IMappingExpression<TicketCategoryCuDto, TicketCategory> mappingExpression)
        {

        }
    }

    public class TicketCategoryListDto : BaseDto<TicketCategoryListDto, TicketCategory, int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public long RoleId { get; set; }
        public string RoleTitle { get; set; }
        public int PriorityId { get; set; }
        public string PriorityTitle { get; set; }
        public int? ParentId { get; set; }
        public string ParentTitle { get; set; }

        public ICollection<TicketCategoryListDto> Children { get; set; }

        public override void CustomMappings(IMappingExpression<TicketCategory, TicketCategoryListDto> mappingExpression)
        {
        }
    }

    public class TicketCategoryBriefListDto : BaseDto<TicketCategoryBriefListDto, TicketCategory, int>
    {
        public string Title { get; set; }
        public ICollection<TicketCategoryBriefListDto> Children { get; set; }
        public override void CustomMappings(IMappingExpression<TicketCategory, TicketCategoryBriefListDto> mappingExpression)
        {

        }
    }

    public class TicketCategorySearchDto : BaseSearchDto//, IHaveCustomExpression<TicketCategory, TicketCategorySearchDto, int>
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }

        public Expression<Func<TicketCategory, bool>> GenerateExpression(TicketCategorySearchDto dto)
        {
            List<Expression<Func<TicketCategory, bool>>> expressions = new List<Expression<Func<TicketCategory, bool>>>();

            if (!string.IsNullOrEmpty(dto.Title))
                expressions.Add(src => src.Title.Contains(dto.Title));

            if (!string.IsNullOrEmpty(dto.LatinTitle))
                expressions.Add(src => src.LatinTitle.Contains(dto.LatinTitle));

            return ExpressionsHelper.AndAll(expressions);
        }
    }
}
