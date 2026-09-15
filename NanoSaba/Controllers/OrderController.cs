using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Application.People.Commands.UpdateOrder;
using CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NanoSaba.Utility;

namespace NanoSaba.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var orders = await _mediator.Send(new GetOrderWithPaginationQuery()
            {
                PageNumber = pageNumber,
                RecordsPerPage = pageSize
            });

            var pagedto = new OrderPageDto()
            {
                Orders = new PaginatedList<OrderBriefDto>(orders.Items, orders.TotalCount, pageNumber, pageSize)
            };

            return View("~/Views/Order/Order.cshtml", pagedto);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmFinancialCommand()
        {
            var orderId = FormAttributesUtility.GetLongByKey("id", Request);
            var description = Request.Form["description"].ToString();
            var processingTime = FormAttributesUtility.GetDoubleByKey("processingTime", Request);

            long result = 0;
            result = await _mediator.Send(new ConfirmFinancialCommand()
            {
                TechnicalComment = description,
                ProcessingTime = processingTime,
                Id = orderId
            });
            if (result > 0)
            {
                var orders = await _mediator.Send(new GetOrderWithPaginationQuery()
                {
                });
                var pagedto = new OrderPageDto()
                {
                    Orders = new PaginatedList<OrderBriefDto>(orders.Items, orders.TotalCount, 1, 10)
                };
                return View("~/Views/Order/Order.cshtml", pagedto);
            }
            else
                return RedirectToAction("Index", "Home", new BaseViewModel()
                {
                    IsSuccess = false,
                    Message = "Error"
                });
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmTechnicalExpertCommand(string button)
        {
            var orderId = FormAttributesUtility.GetLongByKey("id", Request);
            var description = Request.Form["description"].ToString();
            long result = 0;
            if (button.Equals("reject"))
            {
                result = await _mediator.Send(new RejectTechnicalExpertCommand()
                {
                    TechnicalComment = description,
                    Id = orderId
                });
            }
            else
            {
                result = await _mediator.Send(new ConfirmTechnicalExpertCommand()
                {
                    TechnicalComment = description,
                    Id = orderId
                });
            }

            if (result > 0)
            {
                var orders = await _mediator.Send(new GetOrderWithPaginationQuery()
                {
                });
                var pagedto = new OrderPageDto()
                {
                    Orders = new PaginatedList<OrderBriefDto>(orders.Items, orders.TotalCount, 1, 10)
                };
                return View("~/Views/Order/Order.cshtml", pagedto);
            }
            else
                return RedirectToAction("Index", "Home", new BaseViewModel()
                {
                    IsSuccess = false,
                    Message = "Error"
                });
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmGettingAnalyze(string button)
        {
            var orderId = FormAttributesUtility.GetLongByKey("id", Request);
            var description = Request.Form["description"].ToString();
            long result = 0;
            if (button.Equals("reject"))
            {
                result = await _mediator.Send(new RejectTechnicalExpertCommand()
                {
                    TechnicalComment = description,
                    Id = orderId
                });
            }
            else
            {
                result = await _mediator.Send(new ConfirmOfGetAnalyzeCommand()
                {
                    TechnicalComment = description,
                    Id = orderId
                });
            }

            if (result > 0)
            {
                var orders = await _mediator.Send(new GetOrderWithPaginationQuery()
                {
                });
                var pagedto = new OrderPageDto()
                {
                    Orders = new PaginatedList<OrderBriefDto>(orders.Items, orders.TotalCount, 1, 10)
                };
                return View("~/Views/Order/Order.cshtml", pagedto);
            }
            else
                return RedirectToAction("Index", "Home", new BaseViewModel()
                {
                    IsSuccess = false,
                    Message = "Error"
                });
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmSurvey()
        {
            var score = FormAttributesUtility.GetLongByKey("score", Request);
            var orderId = FormAttributesUtility.GetLongByKey("id", Request);
            var comment = Request.Form["comment"].ToString();
            long result = 0;

            result = await _mediator.Send(new SubmitSurveyOfOrderCommand()
            {
                Comment = comment,
                Score = SurveyScore.FromId((int)score),
                OrderId = orderId
            });

            var orders = await _mediator.Send(new GetOrderWithPaginationQuery()
            {
            });
            var pagedto = new OrderPageDto()
            {
                Orders = new PaginatedList<OrderBriefDto>(orders.Items, orders.TotalCount, 1, 10)
            };
            return View("~/Views/Order/Order.cshtml", pagedto);
        }


        [HttpPost]
        public async Task<IActionResult> UploadResultFile(IFormFile UploadedFile)
        {
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                ViewBag.UploadResult = "No file selected.";
                return View("~/Views/Order/Order.cshtml", null);
            }

            string contentType = UploadedFile.FileName;
            string fileFormat = contentType.Split('.')[1];
            var orderId = FormAttributesUtility.GetLongByKey("id", Request);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var image = new CreateImageCommand()
            {
                ImageTitle = $"{orderId}-Result.{fileFormat}",
                ImageLatinTitle = "test",
                ContextId = 1,
                ImageFile = UploadedFile,
                FolderPath = filePath
            };
            var resultFile = await _mediator.Send(image);
            if (resultFile > 0)
            {
                var result = await _mediator.Send(new OrderCompletedCommand()
                {
                    Id = orderId,
                    ImageId = resultFile
                });
                var orders = await _mediator.Send(new GetOrderWithPaginationQuery()
                {
                });
                var pagedto = new OrderPageDto()
                {
                    Orders = new PaginatedList<OrderBriefDto>(orders.Items, orders.TotalCount, 1, 10)
                };
                return View("~/Views/Order/Order.cshtml", pagedto);
            }
            else
                return RedirectToAction("Index", "Home", new BaseViewModel()
                {
                    IsSuccess = false,
                    Message = "Error"
                });
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile UploadedFile)
        {
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                ViewBag.UploadResult = "No file selected.";
                return View("~/Views/Order/Order.cshtml", null);
            }

            var orderId = FormAttributesUtility.GetLongByKey("id", Request);
            string contentType = UploadedFile.ContentType;
            string fileFormat = contentType.Split('/')[1];
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var image = new CreateImageCommand()
            {
                ImageTitle = $"{orderId}-fish.{fileFormat}",
                ImageLatinTitle = "test",
                ContextId = 1,
                ImageFile = UploadedFile,
                FolderPath = filePath
            };
            var resultFile = await _mediator.Send(image);
            if (resultFile > 0)
            {
                var result = await _mediator.Send(new SendOrderToFinancialCommand()
                {
                    Id = orderId,
                    ImageId = resultFile
                });
                var orders = await _mediator.Send(new GetOrderWithPaginationQuery()
                {
                });
                var pagedto = new OrderPageDto()
                {
                    Orders = new PaginatedList<OrderBriefDto>(orders.Items, orders.TotalCount, 1, 10)
                };
                return View("~/Views/Order/Order.cshtml", pagedto);
            }
            else
                return RedirectToAction("Index", "Home", new BaseViewModel()
                {
                    IsSuccess = false,
                    Message = "Error"
                });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmFinancial()
        {
            var orderId = FormAttributesUtility.GetLongByKey("id", Request);
            var analyzePrice = FormAttributesUtility.GeDecimalByKey("analyzePrice", Request);
            var tax = FormAttributesUtility.GeDecimalByKey("tax", Request);
            var grantPrice = FormAttributesUtility.GeDecimalByKey("grantPrice", Request);
            var additionalPrice = FormAttributesUtility.GeDecimalByKey("additionalPrice", Request);
            decimal taxValue = (analyzePrice * tax / 100m);
            decimal totalPayablePrice = (analyzePrice + taxValue + additionalPrice) - grantPrice;
            var result = await _mediator.Send(new SetFinancialOfOrderCommand()
            {
                Id = orderId,
                TotalPayablePrice = totalPayablePrice,
                GrantPrice = grantPrice,
                Tax = tax,
                AnalyzePrice = analyzePrice,
                AdditionalPrice = additionalPrice,
            });
            if (result > 0)
            {
                var orders = await _mediator.Send(new GetOrderWithPaginationQuery()
                {
                });
                var pagedto = new OrderPageDto()
                {
                    Orders = new PaginatedList<OrderBriefDto>(orders.Items, orders.TotalCount, 1, 10)
                };
                return View("~/Views/Order/Order.cshtml", pagedto);
            }
            else
                return RedirectToAction("Index", "Home", new BaseViewModel()
                {
                    IsSuccess = false,
                    Message = "Error"
                });
        }
    }
}