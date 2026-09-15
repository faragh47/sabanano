using Common;
using DataTransferObjects.DataTransferObjects.NotificationDTOs;
using DataTransferObjects.SharedModels;
using Entities.DatabaseModels.NotificationModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

namespace Services.IServices.V2.Notifications
{
    public interface INotificationService : ICrudService<NotificationCuDto, NotificationListDto, NotificationSearchDto, Notification, long>, IScopedDependency
    {
        Task<ApiResult<IPagedList<NotificationListDto>>> GetMyNotifications(NotificationSearchDto searchDto, CancellationToken cancellationToken);
        Task<ApiResult<List<NotificationListDto>>> GetAllMyNotifications(NotificationSearchDto searchDto, CancellationToken cancellationToken);
    }
}
