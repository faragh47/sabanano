using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services.V2.Ticketing
{
    //public class TicketDashboardStatScheduler : ITicketDashboardStatScheduler
    //{
    //    private readonly ITicketService _TicketService;
    //    public TicketDashboardStatScheduler(
    //        ITicketService ticketService)
    //    {
    //        _TicketService = ticketService;
    //    }
    //    public async Task TicketDashboardStatJob(string dvlpConnectionString, CancellationToken cancellationToken)
    //    {
    //        await _TicketService.InitializeAutomaticTicketDashboard(cancellationToken);
            
    //        //await _TicketService.AutomaticTicketClosing(cancellationToken);

    //        //await QueryExtension.ExecuteSP(dvlpConnectionString, "UpdateTicketDashboardTable");
    //    }
    //}
}