using System.Diagnostics;
using System.Security.Claims;
using Azure.Core;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.HrManagment.People.Commands.CreatePersonItem;
using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Infrastructure.Services.SMSProvider.Faraz;
using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.SharedModels;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NanoSaba.Models;
using Services.IServices.V2;

namespace NanoSaba.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetArticleWithPaginationQuery()
            {
                RecordsPerPage = 3
            }, CancellationToken.None);

            Request.HttpContext.User = User;

            return View("Index", new IndexDto()
            {
                Articles = result.Items
            });
        }
    }
}