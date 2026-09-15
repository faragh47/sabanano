using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Application.People.Commands.UpdateOrder;
using CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NanoSaba.Controllers
{
    public class OrderCartController : Controller
    {
        private readonly IMediator _mediator;

        public OrderCartController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SendToTechnicalExpert()
        {
            var result = await _mediator.Send(new SendOrderToTechnicalExpertCommand());
            if (result > 0)
            {
                return Json(new { isSendToTechnicalExpert = true });
            }
            else
                return RedirectToAction("Index", "Home", new BaseViewModel()
                {
                    IsSuccess = false,
                    Message = "Error"
                });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetOrderWithPaginationQuery()
            {
                OrderStatus = OrderStatus.Initial
            });

            var resultArticle = await _mediator.Send(new GetArticleWithPaginationQuery()
            {
                RecordsPerPage = 3
            }, CancellationToken.None);

            var pagedto = new OrderCartPageDto()
            {
                Orders = result.Items,
                Articles = resultArticle.Items
            };

            return View("~/Views/Order/OrderCart.cshtml", pagedto);
        }
    }
}