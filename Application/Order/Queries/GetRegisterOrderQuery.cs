using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.HrManagment;
using CleanArchitecture.Application.Orders;
using CleanArchitecture.Application.Orders.Commands.Create;
using CleanArchitecture.Application.Orders.Queries;
using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Order.Analyze;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.ValueObjects;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.CustomExpressions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.People.Queries.GetPeopleWithPagination;

public record GetRegisterOrderQuery : BaseRecordSearchDto, IRequest<RegisterOrderPageDto>
{
    public Expression<Func<Order, bool>> GenerateExpression(GetRegisterOrderQuery dto)
    {
        List<Expression<Func<Order, bool>>> expressions = new();
        // ExpressionsHelper.GenerateActorsExpression<AccGroup, GroupSearchDto, int>(dto);
        if (Id > 0)
        {
            expressions.Add(src => src.Id.Equals(Id));
        }

        if (CreatorId > 0)
        {
            expressions.Add(src => src.CreatedBy.Equals(CreatorId));
        }

        return ExpressionsHelper.AndAll(expressions);
    }
}

public class
    GetRegisterOrderQueryHandler : IRequestHandler<GetRegisterOrderQuery, RegisterOrderPageDto>
{
    private readonly IRepository<Order> _repository;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;
    private readonly AnalyzeDeviceFinder analyzeDeviceFinder;

    public GetRegisterOrderQueryHandler(IRepository<Order> repository,
        IMapper mapper, IRepository<BET> betRepository,
        IRepository<FTIR> ftirRepository,
        IRepository<AAS> aasRepository,
        IRepository<AFM> afmRepository,
        IRepository<ContactAngle> contactAngleRepository,
        IRepository<EDX> edxRepository,
        IRepository<Mapping> mappingRepository,
        IRepository<FireAssay> fireAssayRepository,
        IRepository<GCMS> gcmsRepository,
        IRepository<AnalyzerDevice> AnalyzerDeviceRepository,
        IRepository<ICPMS> icpmsRepository,
        IRepository<SEM> semRepository,
        IRepository<TEM> temRepository,
        IRepository<ICPOES> icpoesRepository,
        IRepository<TGA> tgaRepository,
        IRepository<UV> uvRepository,
        IRepository<XRD> xrdRepository,
        IRepository<XRF> xrfRepository,
        IIdentityService identityService)
    {
        _repository = repository;
        _mapper = mapper;
        _identityService = identityService;
        analyzeDeviceFinder = new(AnalyzerDeviceRepository,
            betRepository,
            ftirRepository,
            aasRepository,
            afmRepository,
            contactAngleRepository,
            edxRepository,
            mappingRepository,
            fireAssayRepository,
            gcmsRepository,
            icpmsRepository,
            icpoesRepository,
            semRepository,
            temRepository,
            tgaRepository,
            uvRepository,
            xrdRepository,
            xrfRepository);
    }

    public async Task<RegisterOrderPageDto> Handle(GetRegisterOrderQuery request,
        CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var OrderResult = await _repository.TableNoTracking
            .Include(x => x.Financial)
            .Include(x => x.OrderStatus)
            .Include(x => x.Grant)
            .Include(x => x.Company)
            .Include(x => x.OrderAnalyzes)
            .ThenInclude(x => x.AnalyzerDevice)
            .Include(x => x.OrderAnalyzes)
            .ThenInclude(x => x.Safety)
            .Include(x => x.OrderAnalyzes)
            .Include(x => x.Histories)
            .Where(expresion)
            .FirstOrDefaultAsync();
        var orderAnalyzes = new List<OrderAnalyzeCommand>();
        foreach (var orderAnalyze in OrderResult.OrderAnalyzes)
        {
            var orderAnalyzeDto = _mapper.Map<OrderAnalyzeCommand>(orderAnalyze);
            orderAnalyzeDto.Key = orderAnalyze.AnalyzerDevice.Key;
            await analyzeDeviceFinder.InitAnalyze(orderAnalyzeDto, orderAnalyze);
            orderAnalyzeDto.State = analyzeDeviceFinder.GetVariablesByKey(orderAnalyze);
            orderAnalyzes.Add(orderAnalyzeDto);
        }

        var user = _identityService.GetUser((Convert.ToInt64(OrderResult.CreatedBy)));
        var firstOrderAnalyze = OrderResult.OrderAnalyzes.FirstOrDefault();
        RegisterOrderPageDto result = new()
        {
            OrderId = OrderResult.Id,
            Name = firstOrderAnalyze?.AnalyzerDevice?.Name,
            OrderAnalyzes = orderAnalyzes,
            Grant = _mapper.Map<CreateGrantCommand>(OrderResult.Grant),
            Company = _mapper.Map<CreateCompanyCommand>(OrderResult.Company),
            IsRequireHeader = OrderResult.IsRequireHeader,
            IsRequireToReturnSample = OrderResult.IsRequireToReturnSample,
            HasGrant = OrderResult.Grant is not null,
            HasCompany = OrderResult.Company is not null,
            Description = OrderResult.Description,
            OrderNumber = OrderResult.OrderNumber,
            TrackingCode = OrderResult.TrackingCode,
            Financial = _mapper.Map<FinancialListDto>(OrderResult.Financial),
            Histories = _mapper.Map<List<OrderHistoryDto>>(OrderResult.Histories),
            Customer = new CustomerInfo
            {
                FullName = user.FullName,
                NationalCode = user.NationalCode,
                MobileNumber = user.MobileNumber,
                EmailAddress = user.Email,
                Address = user.Address
            },
        };
        return result;
    }
}