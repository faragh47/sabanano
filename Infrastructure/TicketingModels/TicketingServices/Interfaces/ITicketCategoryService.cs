using CleanArchitecture.Infrastructure.TicketingModels.Dto;
using Common;
using DataTransferObjects.SharedModels;
using Entities.DatabaseModels.TicketingModels;
using Microsoft.AspNetCore.Http;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

namespace CleanArchitecture.Infrastructure.TicketingModels.TicketingServices.Ticketing
{
    public interface ITicketCategoryService : ICrudService<TicketCategoryCuDto, TicketCategoryListDto, TicketCategorySearchDto, TicketCategory, int>, IScopedDependency
    {
        Task<ApiResult<List<TicketCategoryBriefListDto>>> GetAll(TicketCategorySearchDto searchDto, CancellationToken cancellationToken);
    }
}
