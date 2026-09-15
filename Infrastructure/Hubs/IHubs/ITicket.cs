using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects.DataTransferObjects.NotificationDTOs;
using DataTransferObjects.SharedModels;

namespace Services.Hubs.IHubs
{
    public interface ITicketHub
    {
        Task SendNotification(ApiResult<NotificationListDto> records);
    }
}
