using CleanArchitecture.Application.Orders.Commands.Create;
using CleanArchitecture.Application.PagesDto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NanoSaba.Controllers;

public class BETController : Controller
{
    private readonly IMediator _mediator;

    public BETController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RemoveItem()
    {
        var analyzeName = TempData["AnaylyzeName"] as string;
        var result = await _mediator.Send(new GetAnalyzerDeviceWithPaginationQuery()
        {
            Name = analyzeName
        });
        var pagedto = new DevicePageDto()
        {
            AnalyzerDevice = result.Items[0],
        };
        return View("~/Views/Order/RegisterOrder.cshtml", null);
    }

   
}