using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.HrManagment;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Entities.Order.Analyze;
using CleanArchitecture.Domain.Entities.Orders;
using CleanArchitecture.Domain.Events;
using Common.Utilities;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.Orders.Commands.Create;

public record CreateOrderCommand : BaseRecordDto<CreateOrderCommand, Order, long>, IRequest<long>
{
    public List<OrderAnalyzeCommand> OrderAnalyzes { get; set; }
    public CreateGrantCommand Grant { get; set; }
    public CreateCompanyCommand Company { get; set; }
    public bool IsRequireHeader { get; set; }
    public string Description { get; set; }
    public bool IsRequireToReturnSample { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
{
    private readonly IRepository<Order> _repository;
    private readonly IRepository<BET> _betRepository;
    private readonly IRepository<FTIR> _ftirRepository;
    private readonly IRepository<AAS> _aasRepository;
    private readonly IRepository<AFM> _afmRepository;
    private readonly IRepository<EDX> _edxRepository;
    private readonly IRepository<Mapping> _mappingRepository;
    private readonly IRepository<FireAssay> _fireAssayRepository;
    private readonly IRepository<ContactAngle> _contactAngleRepository;
    private readonly IRepository<GCMS> _gcmsRepository;
    private readonly IRepository<ICPMS> _icpmsRepository;
    private readonly IRepository<ICPOES> _icpoesRepository;
    private readonly IRepository<SEM> _semRepository;
    private readonly IRepository<TEM> _temRepository;
    private readonly IRepository<TGA> _tgaRepository;
    private readonly IRepository<UV> _uvRepository;
    private readonly IRepository<XRD> _xrdRepository;
    private readonly IRepository<XRF> _xrfRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;
    private readonly AnalyzeDeviceFinder AnalyzeDeviceFinder;

    public CreateOrderCommandHandler(IRepository<Order> repository,
        IMapper mapper,
        IMediator mediator,
        IRepository<AnalyzerDevice> _analyzerDeviceRepository,
        IRepository<BET> betRepository,
        IRepository<FTIR> ftirRepository,
        IIdentityService identityService,
        ICurrentUserService currentUserService,
        IRepository<AAS> aasRepository,
        IRepository<AFM> afmRepository,
        IRepository<ContactAngle> contactAngleRepository,
        IRepository<EDX> edxRepository,
        IRepository<Mapping> mappingRepository,
        IRepository<FireAssay> fireAssayRepository,
        IRepository<GCMS> gcmsRepository, IRepository<ICPMS> icpmsRepository, IRepository<ICPOES> icpoesRepository,
        IRepository<SEM> semRepository,
        IRepository<TEM> temRepository,
        IRepository<TGA> tgaRepository,
        IRepository<UV> uvRepository, IRepository<XRD> xrdRepository, IRepository<XRF> xrfRepository)
    {
        AnalyzeDeviceFinder = new AnalyzeDeviceFinder(_analyzerDeviceRepository);
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
        _betRepository = betRepository;
        _ftirRepository = ftirRepository;
        _identityService = identityService;
        _currentUserService = currentUserService;
        _aasRepository = aasRepository;
        _afmRepository = afmRepository;
        _contactAngleRepository = contactAngleRepository;
        _edxRepository = edxRepository;
        _mappingRepository = mappingRepository;
        _fireAssayRepository = fireAssayRepository;
        _gcmsRepository = gcmsRepository;
        _icpmsRepository = icpmsRepository;
        _icpoesRepository = icpoesRepository;
        _semRepository = semRepository;
        _temRepository = temRepository;
        _tgaRepository = tgaRepository;
        _uvRepository = uvRepository;
        _xrdRepository = xrdRepository;
        _xrfRepository = xrfRepository;
    }

    public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);
        var bets =
            entity.OrderAnalyzes = new List<OrderAnalyze>();
        var orderAnalyzes = await CreateOrderAnalyzes(request.OrderAnalyzes, cancellationToken);
        entity.TrackingCode = CodeGenerator.GenerateNumberCode();
        entity.OrderNumber = CodeGenerator.GenerateNumberCode();
        entity.OrderAnalyzes = orderAnalyzes;
        if (request.Grant is not null)
            entity.GrantId = await _mediator.Send(request.Grant);
        if (request.Company is not null)
            entity.CompanyId = await _mediator.Send(request.Company);
        entity.IsRequireHeader = request.IsRequireHeader;
        entity.Description = request.Description;
        entity.PersonId = await _identityService
            .GetPersonId(Convert.ToInt64(_currentUserService.UserId));
        entity.IsRequireToReturnSample = request.IsRequireToReturnSample;
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    private async Task<List<OrderAnalyze>> CreateOrderAnalyzes(List<OrderAnalyzeCommand> orderAnalyzes,
        CancellationToken cancellationToken)
    {
        List<OrderAnalyze> analyzes = new();
        int counter = 0;
        foreach (var analyze in orderAnalyzes)
        {
            counter++;
            var orderAnalyze = _mapper.Map<OrderAnalyze>(analyze);
            orderAnalyze.TrackingCode = counter.ToString();
            orderAnalyze.Safety = new Safety()
            {
                IsEscapable = analyze.IsEscapable,
                IsFlammable = analyze.IsFlammable,
                IsPoisonous = analyze.IsPoisonous,
                HasNoSafety = analyze.HasNoSafety,
                IsExplosive = analyze.IsExplosive,
                IsSickness = analyze.IsSickness,
                IsNanoSize = analyze.IsNanoSize,
                IsAdsorbBySkin = analyze.IsAdsorbBySkin,
                IsBadForBreathing = analyze.IsBadForBreathing,
            };
            orderAnalyze.AnalyzeModelId = await addModel(analyze, cancellationToken);
            var analyzeDevice = AnalyzeDeviceFinder.Find(analyze.Key);
            orderAnalyze.AnalyzeDeviceId = analyzeDevice.Id;
            analyzes.Add(orderAnalyze);
        }

        return analyzes;
    }

    private async Task<long> addModel(OrderAnalyzeCommand analyze, CancellationToken cancellationToken)
    {
        long result = 0;
        if (analyze.Key.Equals("BET"))
        {
            var model = new BET()
            {
                Time = analyze.Bet.Time,
                DegassingTemperature = analyze.Bet.DegassingTemperature,
            };
            await _betRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("FTIR"))
        {
            var model = new FTIR()
            {
                SampleState = analyze.Ftir.SampleState,
                SampleType = analyze.Ftir.SampleType,
                SampleComposition = analyze.Ftir.SampleComposition,
            };
            await _ftirRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("AAS"))
        {
            var model = new AAS()
            {
                Element = analyze.AAS.Element,
                IsNeedDigesting = analyze.AAS.IsNeedDigesting,
                Description = analyze.AAS.Description,
            };
            await _aasRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("AFM"))
        {
            var model = new AFM()
            {
                Breed = analyze.AFM.Breed,
                IsNeedDigesting = analyze.AFM.IsNeedDigesting,
                Description = analyze.AFM.Description,
            };
            await _afmRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("ContactAngle"))
        {
            var model = new ContactAngle()
            {
                Breed = analyze.ContactAngle.Breed,
                IsNeedDigesting = analyze.ContactAngle.IsNeedDigesting,
                Description = analyze.ContactAngle.Description,
            };
            await _contactAngleRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("EDX"))
        {
            var model = new EDX()
            {
                Element = analyze.EDX.Element,
            };
            await _edxRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("Mapping"))
        {
            var model = new Mapping()
            {
                Element = analyze.Mapping.Element,
            };
            await _mappingRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("FireAssay"))
        {
            var model = new FireAssay()
            {
                Element = analyze.FireAssay.Element,
                Description = analyze.FireAssay.Description,
            };
            await _fireAssayRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("GC-MS"))
        {
            var model = new GCMS()
            {
                SampleNature = analyze.GCMS.SampleNature,
                Solvent = analyze.GCMS.Solvent,
                Composition = analyze.GCMS.Composition,
            };
            await _gcmsRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("ICP-MS"))
        {
            var model = new ICPMS()
            {
                Element = analyze.ICPMS.Element,
                Description = analyze.ICPMS.Description,
            };
            await _icpmsRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("ICP-OES"))
        {
            var model = new ICPOES()
            {
                Element = analyze.ICPOES.Element,
                Description = analyze.ICPOES.Description,
            };
            await _icpoesRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("SEM"))
        {
            var model = new SEM()
            {
                Zoom = analyze.SEM.Zoom,
                Size = analyze.SEM.Size,
                Description = analyze.SEM.Description,
            };
            await _semRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("TEM"))
        {
            var model = new TEM()
            {
                Zoom = analyze.SEM.Zoom,
                Size = analyze.SEM.Size,
                Description = analyze.SEM.Description,
            };
            await _temRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("TGA"))
        {
            var model = new TGA()
            {
                TempertureStart = analyze.TGA.TempertureStart,
                TempertureEnd = analyze.TGA.TempertureEnd,
                Environment = analyze.TGA.Environment,
                Rate = analyze.TGA.Rate,
            };
            await _tgaRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("UV-Vis"))
        {
            var model = new UV()
            {
                Spectrum = analyze.UV.Spectrum,
                WaveLengthEnd = analyze.UV.WaveLengthEnd,
                WaveLengthStart = analyze.UV.WaveLengthStart,
                Type = analyze.UV.Type,
                Solvent = analyze.UV.Solvent,
            };
            await _uvRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("XRD"))
        {
            var model = new XRD()
            {
                AngleStart = analyze.XRD.AngleStart,
                AngleEnd = analyze.XRD.AngleEnd,
                Composition = analyze.XRD.Composition,
                Type = analyze.XRD.Type,
                IsNeedGrind = analyze.XRD.IsNeedGrind,
            };
            await _xrdRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }
        else if (analyze.Key.Equals("XRF"))
        {
            var model = new XRF()
            {
                IsNeedGrind = analyze.XRF.IsNeedGrind,
                IsNeedIOL = analyze.XRF.IsNeedIOL,
                Type = analyze.XRF.Type,
            };
            await _xrfRepository.AddAsync(model, cancellationToken);
            result = model.Id;
        }

        return result;
    }
}