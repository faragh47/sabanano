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
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Orders.Commands.Create;

public record UpdateOrderCommand : BaseRecordDto<UpdateOrderCommand, Order, long>, IRequest<long>
{
    public List<OrderAnalyzeCommand> OrderAnalyzes { get; set; }
    public CreateGrantCommand Grant { get; set; }
    public CreateCompanyCommand Company { get; set; }
    public bool IsRequireHeader { get; set; }
    public bool IsRequireToReturnSample { get; set; }
    public string Description { get; set; }
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, long>
{
    private readonly IRepository<Order> _repository;
    private readonly IRepository<OrderAnalyze> _OrderAnalyzeRepository;
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
    private readonly AnalyzeDeviceFinder AnalyzeDeviceFinder;

    public UpdateOrderCommandHandler(IRepository<Order> repository,
        IMapper mapper,
        IRepository<AnalyzerDevice> _analyzerDeviceRepository, IMediator mediator,
        IRepository<BET> betRepository,
        IRepository<OrderAnalyze> orderAnalyzeRepository,
        IRepository<FTIR> ftirRepository,
        IRepository<AAS> aasRepository,
        IRepository<EDX> edxRepository,
        IRepository<Mapping> mappingRepository,
        IRepository<ContactAngle> contactAngleRepository,
        IRepository<FireAssay> fireAssayRepository,
        IRepository<AFM> afmRepository,
        IRepository<ICPMS> icpmsRepository,
        IRepository<ICPOES> icpoesRepository,
        IRepository<GCMS> gcmsRepository,
        IRepository<SEM> semRepository,
        IRepository<TEM> temRepository,
        IRepository<TGA> tgaRepository,
        IRepository<XRD> xrdRepository,
        IRepository<UV> uvRepository,
        IRepository<XRF> xrfRepository)
    {
        AnalyzeDeviceFinder = new AnalyzeDeviceFinder(_analyzerDeviceRepository,
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
            xrfRepository
        );
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
        _betRepository = betRepository;
        _OrderAnalyzeRepository = orderAnalyzeRepository;
        _ftirRepository = ftirRepository;
        _aasRepository = aasRepository;
        _afmRepository = afmRepository;
        _gcmsRepository = gcmsRepository;
        _semRepository = semRepository;
        _temRepository = temRepository;
        _tgaRepository = tgaRepository;
        _uvRepository = uvRepository;
        _xrfRepository = xrfRepository;
        _contactAngleRepository = contactAngleRepository;
        _icpmsRepository = icpmsRepository;
        _icpoesRepository = icpoesRepository;
        _contactAngleRepository = contactAngleRepository;
        _xrdRepository = xrdRepository;
    }

    public async Task<long> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var exist = await _repository.TableNoTracking
            .Include(x => x.OrderAnalyzes)
            .ThenInclude(x => x.Safety)
            .Include(x => x.OrderAnalyzes)
            .FirstOrDefaultAsync(x => x.Id == request.Id);
        await CreateOrderAnalyzes(request.OrderAnalyzes, exist, cancellationToken);
        if (request.Grant is not null)
            exist.GrantId = await _mediator.Send(request.Grant);
        else
        {
            exist.GrantId = null;
            exist.Grant = null;
        }

        if (request.Company is not null)
            exist.CompanyId = await _mediator.Send(request.Company);
        else
        {
            exist.CompanyId = null;
            exist.Company = null;
        }

        exist.IsRequireHeader = request.IsRequireHeader;
        exist.Description = request.Description;
        exist.IsRequireToReturnSample = request.IsRequireToReturnSample;
        await _repository.UpdateAsync(exist, cancellationToken);
        return exist.Id;
    }

    private async Task CreateOrderAnalyzes(List<OrderAnalyzeCommand> orderAnalyzes,
        Order exist,
        CancellationToken cancellationToken)
    {
        if (exist.OrderAnalyzes is { Count: > 0 })
        {
            foreach (var orderAnalyze in exist.OrderAnalyzes)
            {
                await _OrderAnalyzeRepository.DeleteAsync(orderAnalyze, cancellationToken);
            }
        }

        int counter = 0;
        foreach (var analyze in orderAnalyzes)
        {
            counter++;
            var orderAnalyze = _mapper.Map<OrderAnalyze>(analyze);
            orderAnalyze.TrackingCode = counter.ToString();
            orderAnalyze.Id = 0;
            orderAnalyze.OrderId = exist.Id;
            var analyzeDevice = AnalyzeDeviceFinder.Find(analyze.Key);
            analyze.AnalyzeDeviceId = analyzeDevice.Id;
            orderAnalyze.AnalyzeDeviceId = analyzeDevice.Id;
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
            orderAnalyze.AnalyzeModelId = await updateModel(analyze, cancellationToken);
            await _OrderAnalyzeRepository.AddAsync(orderAnalyze, cancellationToken);
        }
    }

    private async Task<long?> updateModel(OrderAnalyzeCommand analyze, CancellationToken cancellationToken)
    {
        long result = 0;
        if (analyze.Key.Equals("BET"))
        {
            var exist = await _betRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Time = analyze.Bet.Time;
                exist.DegassingTemperature = analyze.Bet.DegassingTemperature;
                exist.Id = exist.Id;
                result = exist.Id;
                await _betRepository.UpdateAsync(exist, cancellationToken);
            }
            else
            {
                var model = new BET()
                {
                    Time = analyze.Bet.Time,
                    DegassingTemperature = analyze.Bet.DegassingTemperature,
                };
                await _betRepository.AddAsync(model, cancellationToken);
                result = model.Id;
            }
        }
        else if (analyze.Key.Equals("FTIR"))
        {
            var exist = await _ftirRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.SampleState = analyze.Ftir.SampleState;
                exist.SampleType = analyze.Ftir.SampleType;
                exist.SampleComposition = analyze.Ftir.SampleComposition;
                exist.Id = exist.Id;
                result = exist.Id;
                await _ftirRepository.UpdateAsync(exist, cancellationToken);
            }
            else
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
        }
        else if (analyze.Key.Equals("AAS"))
        {
            var exist = await _aasRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Element = analyze.AAS.Element;
                exist.IsNeedDigesting = analyze.AAS.IsNeedDigesting;
                exist.Description = analyze.AAS.Description;
                exist.Id = exist.Id;
                result = exist.Id;
                await _aasRepository.UpdateAsync(exist, cancellationToken);
            }
            else
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
        }
        else if (analyze.Key.Equals("AFM"))
        {
            var exist = await _afmRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Breed = analyze.AFM.Breed;
                exist.IsNeedDigesting = analyze.AFM.IsNeedDigesting;
                exist.Description = analyze.AFM.Description;
                exist.Id = exist.Id;
                result = exist.Id;
                await _afmRepository.UpdateAsync(exist, cancellationToken);
            }
            else
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
        }
        else if (analyze.Key.Equals("ContactAngle"))
        {
            var exist = await _contactAngleRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Breed = analyze.ContactAngle.Breed;
                exist.IsNeedDigesting = analyze.ContactAngle.IsNeedDigesting;
                exist.Description = analyze.ContactAngle.Description;
                exist.Id = exist.Id;
                result = exist.Id;
                await _contactAngleRepository.UpdateAsync(exist, cancellationToken);
            }
            else
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
        }
        else if (analyze.Key.Equals("EDX"))
        {
            var exist = await _edxRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Element = analyze.EDX.Element;
                exist.Id = exist.Id;
                result = exist.Id;
                await _edxRepository.UpdateAsync(exist, cancellationToken);
            }
            else
            {
                var model = new EDX()
                {
                    Element = analyze.EDX.Element,
                };
                await _edxRepository.AddAsync(model, cancellationToken);
                result = model.Id;
            }
        }
        else if (analyze.Key.Equals("Mapping"))
        {
            var exist = await _mappingRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Element = analyze.Mapping.Element;
                exist.Id = exist.Id;
                result = exist.Id;
                await _mappingRepository.UpdateAsync(exist, cancellationToken);
            }
            else
            {
                var model = new Mapping()
                {
                    Element = analyze.Mapping.Element,
                };
                await _mappingRepository.AddAsync(model, cancellationToken);
                result = model.Id;
            }
        }
        else if (analyze.Key.Equals("FireAssay"))
        {
            var exist = await _fireAssayRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Element = analyze.FireAssay.Element;
                exist.Description = analyze.FireAssay.Description;
                exist.Id = exist.Id;
                result = exist.Id;
                await _fireAssayRepository.UpdateAsync(exist, cancellationToken);
            }
            else
            {
                var model = new FireAssay()
                {
                    Element = analyze.FireAssay.Element,
                    Description = analyze.FireAssay.Description,
                };
                await _fireAssayRepository.AddAsync(model, cancellationToken);
                result = model.Id;
            }
        }
        else if (analyze.Key.Equals("GC-MS"))
        {
            var exist = await _gcmsRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.SampleNature = analyze.GCMS.SampleNature;
                exist.Solvent = analyze.GCMS.Solvent;
                exist.Composition = analyze.GCMS.Composition;
                exist.Id = exist.Id;
                result = exist.Id;
                await _gcmsRepository.UpdateAsync(exist, cancellationToken);
            }
            else
            {
                var model = new GCMS()
                {
                    Composition = analyze.GCMS.Composition,
                    Solvent = analyze.GCMS.Solvent,
                    SampleNature = analyze.GCMS.SampleNature,
                };
                await _gcmsRepository.AddAsync(model, cancellationToken);
                result = model.Id;
            }
        }
        else if (analyze.Key.Equals("ICP-MS"))
        {
            var exist = await _icpmsRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Element = analyze.FireAssay.Element;
                exist.Description = analyze.FireAssay.Description;
                exist.Id = exist.Id;
                result = exist.Id;
                await _icpmsRepository.UpdateAsync(exist, cancellationToken);
            }
            else
            {
                var model = new ICPMS()
                {
                    Element = analyze.FireAssay.Element,
                    Description = analyze.FireAssay.Description,
                };
                await _icpmsRepository.AddAsync(model, cancellationToken);
                result = model.Id;
            }
        }
        else if (analyze.Key.Equals("ICP-OES"))
        {
            var exist = await _icpoesRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Element = analyze.FireAssay.Element;
                exist.Description = analyze.FireAssay.Description;
                exist.Id = exist.Id;
                result = exist.Id;
                await _icpoesRepository.UpdateAsync(exist, cancellationToken);
            }
            else
            {
                var model = new ICPOES()
                {
                    Element = analyze.FireAssay.Element,
                    Description = analyze.FireAssay.Description,
                };
                await _icpoesRepository.AddAsync(model, cancellationToken);
                result = model.Id;
            }
        }
        else if (analyze.Key.Equals("SEM"))
        {
            var exist = await _semRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Zoom = analyze.SEM.Zoom;
                exist.Size = analyze.SEM.Size;
                exist.Description = analyze.SEM.Description;
                exist.Id = exist.Id;
                result = exist.Id;
                await _semRepository.UpdateAsync(exist, cancellationToken);
            }
            else
            {
                var model = new SEM()
                {
                    Size = analyze.SEM.Size,
                    Zoom = analyze.SEM.Zoom,
                    Description = analyze.SEM.Description,
                };
                await _semRepository.AddAsync(model, cancellationToken);
                result = model.Id;
            }
        }
        else if (analyze.Key.Equals("TEM"))
        {
            var exist = await _temRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Zoom = analyze.TEM.Zoom;
                exist.Size = analyze.TEM.Size;
                exist.Description = analyze.TEM.Description;
                exist.Id = exist.Id;
                result = exist.Id;
                await _temRepository.UpdateAsync(exist, cancellationToken);
            }
            else
            {
                var model = new TEM()
                {
                    Size = analyze.TEM.Size,
                    Zoom = analyze.TEM.Zoom,
                    Description = analyze.TEM.Description,
                };
                await _temRepository.AddAsync(model, cancellationToken);
                result = model.Id;
            }
        }
        else if (analyze.Key.Equals("TGA"))
        {
            var exist = await _tgaRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Environment = analyze.TGA.Environment;
                exist.TempertureStart = analyze.TGA.TempertureStart;
                exist.TempertureEnd = analyze.TGA.TempertureEnd;
                exist.Rate = analyze.TGA.Rate;
                exist.Id = exist.Id;
                result = exist.Id;
                await _tgaRepository.UpdateAsync(exist, cancellationToken);
            }
            else
            {
                var model = new TGA()
                {
                    Environment = analyze.TGA.Environment,
                    TempertureStart = analyze.TGA.TempertureStart,
                    TempertureEnd = analyze.TGA.TempertureEnd,
                    Rate = analyze.TGA.Rate,
                };
                await _tgaRepository.AddAsync(model, cancellationToken);
                result = model.Id;
            }
        }
        else if (analyze.Key.Equals("UV-Vis"))
        {
            var exist = await _uvRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Spectrum = analyze.UV.Spectrum;
                exist.WaveLengthStart = analyze.UV.WaveLengthStart;
                exist.WaveLengthEnd = analyze.UV.WaveLengthEnd;
                exist.Type = analyze.UV.Type;
                exist.Solvent = analyze.UV.Solvent;
                exist.Id = exist.Id;
                result = exist.Id;
                await _uvRepository.UpdateAsync(exist, cancellationToken);
            }
            else
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
        }
        else if (analyze.Key.Equals("XRD"))
        {
            var exist = await _xrdRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.AngleStart = analyze.XRD.AngleStart;
                exist.AngleEnd = analyze.XRD.AngleEnd;
                exist.Composition = analyze.XRD.Composition;
                exist.Type = analyze.XRD.Type;
                exist.IsNeedGrind = analyze.XRD.IsNeedGrind;
                exist.Id = exist.Id;
                result = exist.Id;
                await _xrdRepository.UpdateAsync(exist, cancellationToken);
            }
            else
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
        }
        else if (analyze.Key.Equals("XRF"))
        {
            var exist = await _xrfRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Id == analyze.AnalyzeDeviceId);
            if (exist is not null)
            {
                exist.Type = analyze.XRF.Type;
                exist.IsNeedGrind = analyze.XRF.IsNeedGrind;
                exist.IsNeedIOL = analyze.XRF.IsNeedIOL;
                exist.Id = exist.Id;
                result = exist.Id;
                await _xrfRepository.UpdateAsync(exist, cancellationToken);
            }
            else
            {
                var model = new XRF()
                {
                    Type = analyze.XRF.Type,
                    IsNeedGrind = analyze.XRF.IsNeedGrind,
                    IsNeedIOL = analyze.XRF.IsNeedIOL,
                };
                await _xrfRepository.AddAsync(model, cancellationToken);
                result = model.Id;
            }
        }

        return result;
    }
}