using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.CustomMapping;
using Entities.DatabaseModels.TicketingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.TicketingModels.Dto
{
    //public class TicketReportCuDto : BaseDto<TicketReportCuDto, Ticket, int>
    //{
    //    public string Title { get; set; }
    //    public override void CustomMappings(IMappingExpression<TicketReportCuDto, Ticket> mappingExpression)
    //    {
    //        //mappingExpression.ForMember(dest => dest.entityField,
    //        //        conf => conf.MapFrom(src => src.dtofield));
    //    }
    //}

    public class TicketReportListDto : BaseDto<TicketReportListDto, Ticket, long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public int PriorityId { get; set; }
        public string PriorityTitle { get; set; }
        public int StatusId { get; set; }
        public string StatusTitle { get; set; }

        public long? ReferedUserId { get; set; }
        public string ReferedUserFullName { get; set; }

        public string CreationDate_TimeStamp { get; set; }
        public string ModificationDate_TimeStamp { get; set; }
        public string CreatorFullName { get; set; }

        public ICollection<TicketUserResponseListDto> TicketUserResponses { get; set; }

        public ICollection<TicketStatusHistoryListDto> StatusHistories { get; set; }
        public ICollection<TicketReferListDto> Refers { get; set; }

        public override void CustomMappings(IMappingExpression<Ticket, TicketReportListDto> mappingExpression)
        {
            //mappingExpression.ForMember(dest => dest.dtoField,
            //        conf => conf.MapFrom(src => src.entityfield));

            mappingExpression.ForMember(dest => dest.TicketUserResponses, conf => conf.MapFrom(src => src.TicketUserResponses.OrderBy(p => p.Created)))
                //.ForMember(dest => dest.CreationDate_TimeStamp, conf => conf.MapFrom(src => (src.CreationDate).TimeOfDay))
                .ForMember(dest => dest.Title, conf => conf.MapFrom(src => src.Title))
                .ForMember(dest => dest.CategoryId, conf => conf.MapFrom(src => src.TicketCategoryId))
                .ForMember(dest => dest.CategoryTitle, conf => conf.MapFrom(src => src.TicketCategory.Title))
                //.ForMember(dest => dest.PriorityId, conf => conf.MapFrom(src => src.FTicketPriorityId))
                //.ForMember(dest => dest.PriorityTitle, conf => conf.MapFrom(src => src.TicketPriority.Title))
                .ForMember(dest => dest.StatusId, conf => conf.MapFrom(src => src.TicketStatusId))
                .ForMember(dest => dest.StatusTitle, conf => conf.MapFrom(src => src.TicketStatus.Title))
                .ForMember(dest => dest.ReferedUserId, conf => conf.MapFrom(src => src.ReferedUserId))
                .ForMember(dest => dest.ReferedUserFullName, conf => conf.MapFrom(src => src.ReferedUser != null ? src.ReferedUser.FullName : ""))
                .ForMember(dest => dest.ReferedUserId, conf => conf.MapFrom(src => src.ReferedUserId))
                .ForMember(dest => dest.CreationDate_TimeStamp, conf => conf.MapFrom(src => new DateTimeOffset((DateTime)src.Created).ToUnixTimeSeconds()))
                .ForMember(dest => dest.ModificationDate_TimeStamp, conf => conf.MapFrom(src => new DateTimeOffset((DateTime)src.LastModified).ToUnixTimeSeconds()))
                .ForMember(dest => dest.StatusHistories, conf => conf.MapFrom(src => src.TicketStatusHistories.OrderBy(p => p.Created)))
                .ForMember(dest => dest.CreatorFullName, conf => conf.MapFrom(src => src.ReferedUser.FullName))
                .ForMember(dest => dest.Refers, conf => conf.MapFrom(src => src.TicketRefers.OrderBy(p => p.Created)));

        }
    }

    public class TicketReportSearchDto : BaseSearchDto//, IHaveCustomExpression<Ticket, TicketReportSearchDto, long>
    {
        public string Title { get; set; }
        public long? ReferredUserId { get; set; }
        public int? StatusId { get; set; }
        public long[] DesiredRoleIds { get; set; }

        public Expression<Func<Ticket, bool>> GenerateExpression(TicketReportSearchDto dto)
        {
            List<Expression<Func<Ticket, bool>>> expressions = new List<Expression<Func<Ticket, bool>>>();

            if (!string.IsNullOrEmpty(dto.Title))
                expressions.Add(src => src.Title.Contains(dto.Title));
            if (dto.ReferredUserId.HasValue)
                if (dto.ReferredUserId > 0)
                    expressions.Add(src => src.ReferedUserId == dto.ReferredUserId);
            if (dto.StatusId.HasValue)
                if (dto.StatusId > 0)
                    expressions.Add(src => src.TicketStatusId == dto.StatusId);
            return ExpressionsHelper.AndAll(expressions);
        }
    }
}
