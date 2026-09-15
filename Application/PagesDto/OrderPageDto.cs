using CleanArchitecture.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Application.PagesDto
{
    public class OrderPageDto : BaseViewModel
    {
        [BindProperty] public IFormFile UploadedFile { get; set; }

        public string UploadResult { get; set; }

        public PaginatedList<OrderBriefDto> Orders { get; set; }
    }
}