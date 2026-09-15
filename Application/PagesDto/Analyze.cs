using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Order.Analyze;
using CleanArchitecture.Domain.Entities.Orders;

namespace CleanArchitecture.Application.PagesDto
{
    public class AnalyzePageDto:BaseDto<AnalyzePageDto, OrderAnalyze, long>
    {
        public List<BETAnalyzeDto> Bets { get;set;}=new List<BETAnalyzeDto>();
    }
}