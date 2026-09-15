using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Identity;
using Common.Utilities;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.CustomMapping;
using DataTransferObjects.DataTransferObjects.TicketingDTOs;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using Entities.DatabaseModels.TicketingModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.TicketingModels.Dto
{
    public class TicketCuDto : BaseDto<TicketCuDto, Ticket, long>, IValidatableObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        //public int PriorityId { get; set; }
        public int StatusId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //var date = PersianDateExtensions.ToGregorianDate(Date-Time);

            if (string.IsNullOrEmpty(Title))
                yield return new ValidationResult("وارد کردن عنوان الزامیست");

            if (Equals(CategoryId, 0))
                yield return new ValidationResult("وارد کردن دسته بندی الزامیست");

            //if (int.Equals(PriorityId, 0))
            //    yield return new ValidationResult("وارد کردن اولویت تیکت الزامیست");
        }
        public override void CustomMappings(IMappingExpression<TicketCuDto, Ticket> mappingExpression)
        {
            mappingExpression.ForMember(dest => dest.Title,
                conf => conf.MapFrom(src => src.Title));

            mappingExpression.ForMember(dest => dest.TicketCategoryId,
                conf => conf.MapFrom(src => src.CategoryId));

            //mappingExpression.ForMember(dest => dest.FTicketPriorityId,
            //    conf => conf.MapFrom(src => src.PriorityId));

            mappingExpression.ForMember(dest => dest.TicketStatusId,
                conf => conf.MapFrom(src => src.StatusId));
        }
    }
    public class TicketListDto : BaseDto<TicketListDto, Ticket, long>
    {
        public string Title { get; set; }
        public TicketCategoryListDto Category { get; set; }

        public int StatusId { get; set; }
        public string StatusTitle { get; set; }

        public ApplicationUserBriefistDto ReferUser { get; set; }
        public ApplicationUserBriefistDto ReferrerUser { get; set; }
        public ApplicationUserBriefistDto Creator { get; set; }

        //public string CreationDate_TimeStamp { get; set; }
        //public string ModificationDate_TimeStamp { get; set; }

        public ICollection<TicketUserResponseListDto> TicketUserResponses { get; set; }
        public ICollection<TicketStatusHistoryListDto> StatusHistories { get; set; }
        public ICollection<TicketReferListDto> Refers { get; set; }

        public override void CustomMappings(IMappingExpression<Ticket, TicketListDto> mappingExpression)
        {
            //mappingExpression.ForMember(dest => dest.dtoField,
            //        conf => conf.MapFrom(src => src.entityfield));


            mappingExpression.ForMember(dest => dest.TicketUserResponses, conf => conf.MapFrom(src => src.TicketUserResponses.OrderBy(p => p.Created)))
            //    //.ForMember(dest => dest.CreationDate_TimeStamp, conf => conf.MapFrom(src => (src.CreationDate).TimeOfDay))
               .ForMember(dest => dest.Category, conf => conf.MapFrom(src => src.TicketCategory))
               .ForMember(dest => dest.StatusId, conf => conf.MapFrom(src => src.TicketStatusId))
               .ForMember(dest => dest.StatusTitle, conf => conf.MapFrom(src => src.TicketStatus.Title))
                  //.ForMember(dest => dest.ReferedUserId, conf => conf.MapFrom(src => src.ReferedUserId))
                  //.ForMember(dest => dest.ReferedUserFullName, conf => conf.MapFrom(src => getName(src.ReferedUser)));
                  ////.ForMember(dest => dest.ReferrerUserId, conf => conf.MapFrom(src => src.TicketRefers != null ? (src.TicketRefers
                  //                                                                        .OrderByDescending(r => r.Id)
                  //                                                                        .FirstOrDefault().FromUserId) : 0))
                  //.ForMember(dest => dest.ReferrerUserFullName, conf => conf.MapFrom(src => src.TicketRefers != null ? (src.TicketRefers
                  //                                                                               .OrderByDescending(r => r.Id)
                  //                                                                               .FirstOrDefault().Creator.FullName) : ""));

                  //.ForMember(dest => dest.CreationDate_TimeStamp, conf => conf.MapFrom(src => new DateTimeOffset((DateTime)src.Created).ToUnixTimeSeconds()))
                  //.ForMember(dest => dest.ModificationDate_TimeStamp, conf => conf.MapFrom(src => new DateTimeOffset((DateTime)src.LastModified).ToUnixTimeSeconds()))
                  .ForMember(dest => dest.StatusHistories, conf => conf.MapFrom(src => src.TicketStatusHistories.OrderBy(p => p.Created)))
                  .ForMember(dest => dest.Refers, conf => conf.MapFrom(src => src.TicketRefers.OrderBy(p => p.Created)));

        }

        private static string getName(ApplicationUser? referedUser)
        {
            return "";
        }
    }
    public class TicketBriefListDto : BaseDto<TicketBriefListDto, Ticket, long>
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public int StatusId { get; set; }
        public string StatusTitle { get; set; }
        public long? ReferedUserId { get; set; }
        public string ReferedUserFullName{ get; set; }
        public string CreationDate_TimeStamp { get; set; }
        public string ModificationDate_TimeStamp { get; set; }
        public string CreatorFullName { get; set; }

        public override void CustomMappings(IMappingExpression<Ticket, TicketBriefListDto> mappingExpression)
        {
            mappingExpression
                .ForMember(dest => dest.Title, conf => conf.MapFrom(src => src.Title))
                .ForMember(dest => dest.CategoryId, conf => conf.MapFrom(src => src.TicketCategoryId))
                .ForMember(dest => dest.CategoryTitle, conf => conf.MapFrom(src => src.TicketCategory.Title))
                .ForMember(dest => dest.StatusId, conf => conf.MapFrom(src => src.TicketStatusId))
                .ForMember(dest => dest.StatusTitle, conf => conf.MapFrom(src => src.TicketStatus.Title))
                .ForMember(dest => dest.ReferedUserId, conf => conf.MapFrom(src => src.ReferedUserId))
                .ForMember(dest => dest.ReferedUserFullName, conf => conf.MapFrom(src => src.ReferedUser != null ? src.ReferedUser.FullName : ""))
                .ForMember(dest => dest.CreationDate_TimeStamp, conf => conf.MapFrom(src => new DateTimeOffset((DateTime)src.Created).ToUnixTimeSeconds()))
                .ForMember(dest => dest.ModificationDate_TimeStamp, conf => conf.MapFrom(src => new DateTimeOffset((DateTime)src.LastModified).ToUnixTimeSeconds()))
                .ForMember(dest => dest.CreatorFullName, conf => conf.MapFrom(src => src.ReferedUser.FullName));
        }
    }

    public class TicketReportDto
    {
        public int AllTicketsCount { get; set; }
        public int OpenTicketsCount { get; set; }
        public int InProgressTicketsCount { get; set; }
        public int ClosedTicketsCount { get; set; }

        public List<TicketReportUserDto> TicketReportUserDto { get; set; }
        public List<TicketReportCategoryDto> TicketReportCategoryDto { get; set; }
    }
    public class TicketReportUserDto
    {
        public long? UserId { get; set; }
        public string FullName { get; set; }
        public int InProgress { get; set; }
        public int Closed { get; set; }
    }
    public class TicketReportCategoryDto
    {
        public long CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public int AllTicketsCount { get; set; }
        public int OpenTicketsCount { get; set; }
        public int InProgressTicketsCount { get; set; }
        public int ClosedTicketsCount { get; set; }
    }
    public class TicketSearchDto : BaseSearchDto//, IHaveCustomExpression<TikTicket, TicketSearchDto, long>
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public string CreatorName { get; set; }
        public long? ReferredUserId { get; set; }
        public long? ReferringUserId { get; set; }
        public int? StatusId { get; set; }
        public long[] DesiredRoleIds { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public long? CreatedBy { get; set; }

        public Expression<Func<Ticket, bool>> GenerateExpression(TicketSearchDto dto)
        {
            List<Expression<Func<Ticket, bool>>> expressions = new List<Expression<Func<Ticket, bool>>>();//ExpressionsHelper.GenerateActorsExpression<TikTicket, TicketSearchDto, long>(dto);

            if (!string.IsNullOrEmpty(dto.FromDate))
                expressions.Add(src => src.Created.Date >= dto.FromDate.ToGregorianDate());

            if (!string.IsNullOrEmpty(dto.ToDate))
                expressions.Add(src => src.Created.Date <= dto.ToDate.ToGregorianDate());
            /*
            if (dto.FromDate is not null)
                expressions.Add(src => src.CreationDate >= dto.FromDate.ToGregorianDate());

            if (dto.ToDate is not null)
                expressions.Add(src => src.CreationDate <= dto.ToDate.ToGregorianDate());
            */
            if (dto.Id.HasValue)
                if (dto.Id > 0)
                    expressions.Add(src => src.Id == dto.Id);
            if (!string.IsNullOrEmpty(dto.Title))
                expressions.Add(src => src.Title.Contains(dto.Title));
            if (dto.CategoryId.HasValue)
                if (dto.CategoryId > 0)
                    expressions.Add(src => src.TicketCategoryId == dto.CategoryId);
            if (!string.IsNullOrEmpty(dto.CreatorName))
                expressions.Add(src => src.Creator.Person.FirstName.Contains(dto.CreatorName));
            if (dto.ReferredUserId.HasValue)
                if (dto.ReferredUserId > 0)
                    expressions.Add(src => src.ReferedUserId == dto.ReferredUserId);
            if (dto.ReferringUserId.HasValue)
                if (dto.ReferringUserId > 0)
                    expressions.Add(src => src.TicketRefers.Any(c => c.CreatedBy == dto.ReferringUserId));
            if (dto.StatusId.HasValue)
                if (dto.StatusId > 0)
                    expressions.Add(src => src.TicketStatusId == dto.StatusId);
            return ExpressionsHelper.AndAll(expressions);
        }
    }
}