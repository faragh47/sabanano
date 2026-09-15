using System.Reflection;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Entities.Articles;
using CleanArchitecture.Infrastructure.Services;
using Common;
using Common.Exceptions;
using Data.Contracts;
using DataTransferObjects.SharedModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NanoSaba.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IRepository<Article> _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ArticleController(IRepository<Article> repository,IMapper mapper,
            IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View("~/Views/Article/Article.cshtml");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ArticleById(int myParams)
        {
            var exist =await _mediator.Send(new GetArticlePageQuery()
            {
                 Id= myParams
            });
            ViewBag.MobileNumber = myParams;
            TempData["Id"] = myParams;
            exist.IsSuccess = true;

            return View("~/Views/Article/Article.cshtml",
              exist);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddComment()
        {
            var Name = Request.Form["Name"].ToString();
            var Email = Request.Form["Email"].ToString();
            var Comment = Request.Form["Comment"].ToString();
            var Id = TempData["Id"];
            ApiResult apiResult = new ApiResult(true, ApiResultStatusCode.Success, null);
            try
            {
                var result = await _mediator.Send(new CreateArticleCommentCommand()
                {
                    Comment = Comment,
                    IssuerEmail = Email,
                    IssuerName=Name,
                    ArticleId = Convert.ToInt32(Id)
                }, CancellationToken.None);
            }
            catch (BadRequestException ex)
            {
                apiResult.Message = ex.Message;
                apiResult.IsSuccess = false;
                return View("Article");
            }

            var exist = await _mediator.Send(new GetArticlePageQuery()
            {
                Id =  Convert.ToInt32(Id)
            });

            exist.IsSuccess = true;

            return View("Article", exist);
        }

    }
}
