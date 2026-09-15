using CleanArchitecture.Application.PagesDto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NanoSaba.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IMediator _mediator;

        public DeviceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetAnalyzerDeviceWithPaginationQuery()
            {
                Name = "BET"
            });

            var pagedto = new DevicePageDto()
            {
                AnalyzerDevice = result.Items[0]
            };

            return View("~/Views/Device/Device.cshtml", pagedto);
        }

        [AllowAnonymous]
        public async Task<IActionResult> DeviceById(string myParams)
        {
            var result = await _mediator.Send(new GetAnalyzerDeviceWithPaginationQuery()
            {
                Name = myParams
            });

            var resultArticle = await _mediator.Send(new GetArticleWithPaginationQuery()
            {
                RecordsPerPage = 3
            }, CancellationToken.None);

            var pagedto = new DevicePageDto()
            {
                AnalyzerDevice = result.Items[0],
                Articles= resultArticle.Items
            };

            return View("~/Views/Device/Device.cshtml", pagedto);
        }
    }
}
