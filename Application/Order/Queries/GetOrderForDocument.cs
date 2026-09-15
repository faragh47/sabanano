using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.HrManagment;
using CleanArchitecture.Application.Orders;
using CleanArchitecture.Application.Orders.Commands.Create;
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

public record GetOrderForDocumentQuery : BaseRecordSearchDto, IRequest<LabAnalysisForm>
{
    public Expression<Func<Order, bool>> GenerateExpression(GetOrderForDocumentQuery dto)
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
    GetOrderForDocumentQueryHandler : IRequestHandler<GetOrderForDocumentQuery, LabAnalysisForm>
{
    private readonly IRepository<Order> _repository;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;
    private readonly AnalyzeDeviceFinder analyzeDeviceFinder;

    public GetOrderForDocumentQueryHandler(IRepository<Order> repository,
        IMapper mapper, IRepository<BET> betRepository, IRepository<AnalyzerDevice> AnalyzerDeviceRepository,
        IIdentityService identityService,
        IRepository<FTIR> ftirRepository,
        IRepository<AFM> afmRepository,
        IRepository<EDX> edxRepository,
        IRepository<Mapping> mappingRepository,
        IRepository<ContactAngle> contactAngleRepository,
        IRepository<FireAssay> fireAssayRepository,
        IRepository<GCMS> gcmsRepository,
        IRepository<ICPMS> icpmsRepository,
        IRepository<SEM> semRepository,
        IRepository<TEM> temRepository,
        IRepository<ICPOES> icpoesRepository,
        IRepository<AAS> aasRepository,
        IRepository<UV> uvRepository,
        IRepository<XRD> xrdRepository,
        IRepository<XRF> xrfRepository,
        IRepository<TGA> tgaRepository)
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

    public async Task<LabAnalysisForm> Handle(GetOrderForDocumentQuery request,
        CancellationToken cancellationToken)
    {
        var expresion = request.GenerateExpression(request);
        var OrderResult = await _repository.TableNoTracking
            .Include(x => x.OrderStatus)
            .Include(x => x.Financial)
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
        var user = _identityService.GetUser((Convert.ToInt64(OrderResult.CreatedBy)));
        foreach (var orderAnalyze in OrderResult.OrderAnalyzes)
        {
            var orderAnalyzeDto = _mapper.Map<OrderAnalyzeCommand>(orderAnalyze);
            orderAnalyzeDto.Key = orderAnalyze.AnalyzerDevice.Key;
            await analyzeDeviceFinder.InitAnalyze(orderAnalyzeDto, orderAnalyze);
            orderAnalyzes.Add(orderAnalyzeDto);
        }

        var firstOrderAnalyze = OrderResult.OrderAnalyzes.FirstOrDefault();
        var result = new LabAnalysisForm
        {
            DocumentNumber = firstOrderAnalyze.AnalyzerDevice.DocumentCode,
            TrackingCode = OrderResult.TrackingCode,
            AcceptanceDate = DateTime.Now,
            FormTitle = $"{firstOrderAnalyze.AnalyzerDevice.Name}فرم درخواست آنالیز ",
            Customer = new CustomerInfo
            {
                FullName = user.FullName,
                NationalCode = user.NationalCode,
                MobileNumber = user.MobileNumber,
                EmailAddress = user.Email,
                Address = user.Address
            },
            PaymentDetails = _mapper.Map<CostsAndPayments>(OrderResult.Financial),
            Grant = _mapper.Map<GrantInfo>(OrderResult.Grant),
            Company = _mapper.Map<CompanyInfo>(OrderResult.Company),
            AdditionalComments = OrderResult.Description,
            LabCompletionDetails = addTechnicalExpert(OrderResult),
            Address =
                "آدرس: طرشت، میدان شهید تیموری، خیابان لطفعلی خانی، انتهای خیابان پارس، خیابان ذوقی، پلاک 22، واحد 2",
            ContactNumber = "شماره تماس: ۰۲۱-۶۶۵۱۹۶۶۶"
        };
        result.Analyzes = addAnalyzes(OrderResult);
        if (result.Grant is not null)
            result.Grant.HasGrant = OrderResult.GrantId is not null;
        return result;
    }

    private LabCompletionInfo addTechnicalExpert(Order orderResult)
    {
        var technicalExpert =
            orderResult.Histories?
                .Where(x => x.OrderStatus.Id == OrderStatus.TechnicalConfirm.Id)
                .FirstOrDefault();
        if (technicalExpert is null)
        {
            technicalExpert = orderResult.Histories?
                .Where(x => x.OrderStatus.Id == OrderStatus.Canceled.Id)
                .FirstOrDefault();
            if (technicalExpert is not null)
            {
                return new LabCompletionInfo()
                {
                    CanPerformTest = false,
                    ExpertOpinion = technicalExpert.TechnicalComment
                };
            }
            else
            {
                return null;
            }
        }

        return new LabCompletionInfo()
        {
            CanPerformTest = true,
            ExpertOpinion = technicalExpert.TechnicalComment
        };
    }


    private List<DocumentOrderAnalyze> addAnalyzes(Order orderResult)
    {
        int counter = 0;
        List<DocumentOrderAnalyze> orderAnalyzes = new();
        foreach (var item in orderResult.OrderAnalyzes)
        {
            counter++;
            var descriptions = new List<string>();
            if (item.IsSensitiveToLight)
                descriptions.Add("حساس به نور");
            if (item.IsSensitiveToHumidity)
                descriptions.Add("حساس به رطوبت");
            if (item.HasNotAnyCondition)
                descriptions.Add("هیچ شرایط خاصی ندارد");
            if (item.SpeceficTemperture.HasValue)
                descriptions.Add($"دمای خاص: {item.SpeceficTemperture.Value} درجه");
            if (item.SpeceficAtmosphere.HasValue)
                descriptions.Add($"فشار خاص: {item.SpeceficAtmosphere.Value} اتمسفر");
            if (item.Safety.HasNoSafety)
                descriptions.Add("فاقد ایمنی");
            if (item.Safety.IsPoisonous)
                descriptions.Add("سمی است");
            if (item.Safety.IsEscapable)
                descriptions.Add("فرار است");
            if (item.Safety.IsFlammable)
                descriptions.Add("قابل اشتعال است");
            if (item.Safety.IsBadForBreathing)
                descriptions.Add("برای تنفس مضر است");
            if (item.Safety.IsAdsorbBySkin)
                descriptions.Add("از طریق پوست جذب می‌شود");
            if (item.Safety.IsNanoSize)
                descriptions.Add("در اندازه نانو است");
            if (item.Safety.IsSickness)
                descriptions.Add("بیماری‌زا است");
            if (item.Safety.IsExplosive)
                descriptions.Add("انفجاری است");
            var description = descriptions.Count > 0 ? string.Join(" و ", descriptions) : "";
            var state = analyzeDeviceFinder.GetVariablesByKey(item);
            orderAnalyzes.Add(new DocumentOrderAnalyze()
            {
                RowNumber = counter,
                SampleName = item.Name,
                SampleCode = item.TrackingCode,
                State = state,
                Description = description
            });
        }

        return orderAnalyzes;
    }
}