using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing
{
    public interface ITicketDashboardStatScheduler : IScopedDependency
    {
        Task TicketDashboardStatJob(string dvlpConnectionString, CancellationToken cancellationToken);
    }
}
