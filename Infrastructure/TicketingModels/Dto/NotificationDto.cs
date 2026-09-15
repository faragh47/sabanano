using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using Common.Utilities;
using DataTransferObjects.CustomExpressions;
using DataTransferObjects.CustomMapping;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using Entities.DatabaseModels.NotificationModels;
using Newtonsoft.Json;

namespace DataTransferObjects.DataTransferObjects.NotificationDTOs
{
    public class NotificationCuDto : BaseDto<NotificationCuDto, Notification, long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long? UserId { get; set; }
        public long? PersonId { get; set; }
        public int? FrontendRouteId { get; set; }
        public int? ContextId { get; set; }
        public bool SendSMS { get; set; }
        public bool SendInApp { get; set; }
        public string SendDate { get; set; }
        public bool Seen { get; set; }

        public override void CustomMappings(IMappingExpression<NotificationCuDto, Notification> mappingExpression)
        {
            mappingExpression.ForMember(dest => dest.Title, conf => conf.MapFrom(src => src.Title));
            mappingExpression.ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description));
            mappingExpression.ForMember(dest => dest.UserId, conf => conf.MapFrom(src => src.UserId));
            mappingExpression.ForMember(dest => dest.PersonId, conf => conf.MapFrom(src => src.PersonId));
            mappingExpression.ForMember(dest => dest.ContextId, conf => conf.MapFrom(src => src.ContextId));
            mappingExpression.ForMember(dest => dest.SendSMS, conf => conf.MapFrom(src => src.SendSMS));
            mappingExpression.ForMember(dest => dest.SendInApp, conf => conf.MapFrom(src => src.SendInApp));
            mappingExpression.ForMember(dest => dest.SendDate, conf => conf.MapFrom(src => PersianDateExtensions.ToGregorianDate((string)src.SendDate)));
            mappingExpression.ForMember(dest => dest.Seen, conf => conf.MapFrom(src => src.Seen));
        }
    }
    public class NotificationListDto : BaseDto<NotificationListDto, Notification, long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long? UserId { get; set; }
        public string FullName { get; set; }
        public long? PersonId { get; set; }
        public int FrontendRouteId { get; set; }
        public int ContextId { get; set; }
        public bool SendSMS { get; set; }
        public bool SendInApp { get; set; }
        public string SendDate { get; set; }
        public bool Seen { get; set; }

        public override void CustomMappings(IMappingExpression<Notification, NotificationListDto> mappingExpression)
        {
            mappingExpression.ForMember(dest => dest.Title, conf => conf.MapFrom(src => src.Title));
            mappingExpression.ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description));
            mappingExpression.ForMember(dest => dest.UserId, conf => conf.MapFrom(src => src.UserId));
            mappingExpression.ForMember(dest => dest.FullName, conf => conf.MapFrom(src => src.Creator.FullName));
            mappingExpression.ForMember(dest => dest.PersonId, conf => conf.MapFrom(src => src.PersonId));
            mappingExpression.ForMember(dest => dest.ContextId, conf => conf.MapFrom(src => src.ContextId));
            mappingExpression.ForMember(dest => dest.SendSMS, conf => conf.MapFrom(src => src.SendSMS));
            mappingExpression.ForMember(dest => dest.SendInApp, conf => conf.MapFrom(src => src.SendInApp));
            mappingExpression.ForMember(dest => dest.SendDate, conf => conf.MapFrom(src => PersianDateExtensions.ToPersianDate(src.SendDate)));
            mappingExpression.ForMember(dest => dest.Seen, conf => conf.MapFrom(src => src.Seen));
        }
    }
    public class NotificationSearchDto : BaseSearchDto//, IHaveCustomExpression<Notification, NotificationSearchDto, long>
    {
        public string? Title { get; set; }
        public long? UserId { get; set; }
        public long? PersonId { get; set; }
        public bool? SendSMS { get; set; }
        public bool? SendInApp { get; set; }
        public DateTime? SendDate { get; set; }
        public bool? Seen { get; set; }

        public Expression<Func<Notification, bool>> GenerateExpression(NotificationSearchDto dto)
        {
            List<Expression<Func<Notification, bool>>> expressions = new List<Expression<Func<Notification, bool>>>(); //ExpressionsHelper.GenerateActorsExpression<Notification, NotificationSearchDto, long>(dto);

            if (!String.IsNullOrEmpty(dto.Title))
                expressions.Add(src => src.Title.Contains(dto.Title));

            if (dto.UserId.HasValue)
                if (dto.UserId > 0)
                    expressions.Add(src => src.UserId == dto.UserId);

            if (dto.PersonId.HasValue)
                if (dto.PersonId > 0)
                    expressions.Add(src => src.PersonId == dto.PersonId);

            if (dto.SendSMS.HasValue)
                expressions.Add(src => src.SendSMS == dto.SendSMS);

            if (dto.SendInApp.HasValue)
                expressions.Add(src => src.SendInApp == dto.SendInApp);

            if (dto.SendDate.HasValue)
                expressions.Add(src => src.SendDate == dto.SendDate);

            if (dto.Seen.HasValue)
                expressions.Add(src => src.Seen == dto.Seen);

            return ExpressionsHelper.AndAll(expressions);
        }
    }
}
