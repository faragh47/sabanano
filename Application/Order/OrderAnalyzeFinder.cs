using CleanArchitecture.Application.Orders.Commands.Create;
using CleanArchitecture.Application.PagesDto;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.Order.Analyze;
using CleanArchitecture.Domain.Entities.Orders;
using Data.Contracts;

namespace CleanArchitecture.Application.Orders;

public class AnalyzeDeviceFinder
{
    private readonly IRepository<AnalyzerDevice> _repository;
    private readonly IRepository<BET> _betRepository;
    private readonly IRepository<FTIR> _ftirRepository;
    private readonly IRepository<AAS> _aasRepository;
    private readonly IRepository<AFM> _afmRepository;
    private readonly IRepository<ContactAngle> _contactAngleRepository;
    private readonly IRepository<EDX> _edxRepository;
    private readonly IRepository<Mapping> _mappingRepository;
    private readonly IRepository<FireAssay> _fireAssayRepository;
    private readonly IRepository<GCMS> _gcmsRepository;
    private readonly IRepository<ICPMS> _icpmsRepository;
    private readonly IRepository<ICPOES> _icpoesRepository;
    private readonly IRepository<SEM> _semRepository;
    private readonly IRepository<TEM> _temRepository;
    private readonly IRepository<TGA> _tgaRepository;
    private readonly IRepository<UV> _uvRepository;
    private readonly IRepository<XRD> _xrdRepository;
    private readonly IRepository<XRF> _xrfRepository;

    public AnalyzeDeviceFinder(IRepository<AnalyzerDevice> repository,
        IRepository<BET> betRepository,
        IRepository<FTIR> ftirRepository,
        IRepository<AAS> aasRepository, IRepository<AFM> afmRepository,
        IRepository<ContactAngle> contactAngleRepository,
        IRepository<EDX> edxRepository,
        IRepository<Mapping> mappingRepository,
        IRepository<FireAssay> fireAssayRepository,
        IRepository<GCMS> gcmsRepository,
        IRepository<ICPMS> icpmsRepository,
        IRepository<ICPOES> icpoesRepository,
        IRepository<SEM> semRepository,
        IRepository<TEM> temRepository,
        IRepository<TGA> tgaRepository,
        IRepository<UV> uvRepository,
        IRepository<XRD> xrdRepository, IRepository<XRF> xrfRepository)
    {
        _repository = repository;
        _betRepository = betRepository;
        _ftirRepository = ftirRepository;
        _aasRepository = aasRepository;
        _afmRepository = afmRepository;
        _contactAngleRepository = contactAngleRepository;
        _edxRepository = edxRepository;
        _mappingRepository = mappingRepository;
        _fireAssayRepository = fireAssayRepository;
        _icpmsRepository = icpmsRepository;
        _icpoesRepository = icpoesRepository;
        _semRepository = semRepository;
        _temRepository = temRepository;
        _tgaRepository = tgaRepository;
        _uvRepository = uvRepository;
        _xrdRepository = xrdRepository;
        _xrfRepository = xrfRepository;
        _gcmsRepository = gcmsRepository;
    }

    public AnalyzeDeviceFinder(IRepository<AnalyzerDevice> repository)
    {
        _repository = repository;
    }

    public string GetVariablesByKey(OrderAnalyze orderAnalyze)
    {
        string result = "";
        if (orderAnalyze.AnalyzerDevice.Key.Equals("BET"))
        {
            result = GetBet(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("FTIR"))
        {
            result = GetFTIR(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("AAS"))
        {
            result = GetAAS(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("AFM"))
        {
            result = GetAFM(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("ContactAngle"))
        {
            result = GetContactAngle(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("EDX"))
        {
            result = GetEDX(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("Mapping"))
        {
            result = GetMapping(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("Mapping"))
        {
            result = GetMapping(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("FireAssay"))
        {
            result = GetFireAssay(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("FireAssay"))
        {
            result = GetFireAssay(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("GC-MS"))
        {
            result = GetGCMS(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("ICP-MS"))
        {
            result = GetICPMS(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("ICP-OES"))
        {
            result = GetICPOES(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("SEM"))
        {
            result = GetSEM(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("TEM"))
        {
            result = GetTEM(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("TGA"))
        {
            result = GetTGA(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("UV-Vis"))
        {
            result = GetUV(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("XRD"))
        {
            result = GetXRD(orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("XRF"))
        {
            result = GetXRF(orderAnalyze);
        }

        return result;
    }

    public async Task InitAnalyze(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        if (orderAnalyze.AnalyzerDevice.Key.Equals("BET"))
        {
            await initBet(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("FTIR"))
        {
            await initFTIR(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("AAS"))
        {
            await initAAS(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("AFM"))
        {
            await initAFM(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("ContactAngle"))
        {
            await initContactAngle(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("EDX"))
        {
            await initEDX(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("Mapping"))
        {
            await initMapping(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("FireAssay"))
        {
            await initFireAssay(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("GC-MS"))
        {
            await initGCMS(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("GC-MS"))
        {
            await initGCMS(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("GC-MS"))
        {
            await initGCMS(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("ICP-MS"))
        {
            await initICPMS(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("ICP-OES"))
        {
            await initICPOES(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("SEM"))
        {
            await initSEM(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("TEM"))
        {
            await initTEM(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("TGA"))
        {
            await initTGA(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("UV-Vis"))
        {
            await initUV(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("XRD"))
        {
            await initXRD(orderAnalyzeDto, orderAnalyze);
        }
        else if (orderAnalyze.AnalyzerDevice.Key.Equals("XRF"))
        {
            await initXRF(orderAnalyzeDto, orderAnalyze);
        }
    }

    private async Task initFTIR(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var ftir = _ftirRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (ftir is not null)
        {
            orderAnalyzeDto.Ftir = new FTIRAnalyzeDto()
            {
                Name = orderAnalyze.Name,
                SampleState = ftir.SampleState,
                SampleType = ftir.SampleType,
                SampleComposition = ftir.SampleComposition,
                Id = ftir.Id
            };
        }
    }

    private async Task initUV(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var exist = _uvRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (exist is not null)
        {
            orderAnalyzeDto.UV = new UVAnalyzeDto()
            {
                Solvent = exist.Solvent,
                WaveLengthStart = exist.WaveLengthStart,
                WaveLengthEnd = exist.WaveLengthEnd,
                Type = exist.Type,
                Spectrum = exist.Spectrum,
                Id = exist.Id
            };
        }
    }

    private async Task initXRD(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var exist = _xrdRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (exist is not null)
        {
            orderAnalyzeDto.XRD = new XRDAnalyzeDto()
            {
                AngleStart = exist.AngleStart,
                AngleEnd = exist.AngleEnd,
                Composition = exist.Composition,
                IsNeedGrind = exist.IsNeedGrind,
                Type = exist.Type,
                Id = exist.Id
            };
        }
    }

    private async Task initGCMS(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var gcms = _gcmsRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (gcms is not null)
        {
            orderAnalyzeDto.GCMS = new GCMSnalyzeDto()
            {
                Composition = gcms.Composition,
                SampleNature = gcms.SampleNature,
                Solvent = gcms.Solvent,
                Id = gcms.Id
            };
        }
    }

    private async Task initAAS(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var aas = _aasRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (aas is not null)
        {
            orderAnalyzeDto.AAS = new AASAnalyzeDto()
            {
                Element = aas.Element,
                IsNeedDigesting = aas.IsNeedDigesting,
                Description = aas.Description,
                Id = aas.Id
            };
        }
    }

    private async Task initEDX(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var model = _edxRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (model is not null)
        {
            orderAnalyzeDto.EDX = new EDXAnalyzeDto()
            {
                Element = model.Element,
                Id = model.Id
            };
        }
    }

    private async Task initMapping(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var model = _mappingRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (model is not null)
        {
            orderAnalyzeDto.Mapping = new MappingAnalyzeDto()
            {
                Element = model.Element,
                Id = model.Id
            };
        }
    }

    private async Task initFireAssay(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var model = _fireAssayRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (model is not null)
        {
            orderAnalyzeDto.FireAssay = new FireAssayAnalyzeDto()
            {
                Element = model.Element,
                Description = model.Description,
                Id = model.Id
            };
        }
    }

    private async Task initICPMS(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var model = _icpmsRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (model is not null)
        {
            orderAnalyzeDto.ICPMS = new ICPMSAnalyzeDto()
            {
                Element = model.Element,
                Description = model.Description,
                Id = model.Id
            };
        }
    }

    private async Task initICPOES(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var model = _icpoesRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (model is not null)
        {
            orderAnalyzeDto.ICPOES = new ICPOESAnalyzeDto()
            {
                Element = model.Element,
                Description = model.Description,
                Id = model.Id
            };
        }
    }

    private async Task initSEM(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var model = _semRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (model is not null)
        {
            orderAnalyzeDto.SEM = new SEMAnalyzeDto()
            {
                Zoom = model.Zoom,
                Size = model.Size,
                Description = model.Description,
                Id = model.Id
            };
        }
    }

    private async Task initTEM(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var model = _temRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (model is not null)
        {
            orderAnalyzeDto.TEM = new TEMAnalyzeDto()
            {
                Zoom = model.Zoom,
                Size = model.Size,
                Description = model.Description,
                Id = model.Id
            };
        }
    }

    private async Task initAFM(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var afm = _afmRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (afm is not null)
        {
            orderAnalyzeDto.AFM = new AFMAnalyzeDto()
            {
                Breed = afm.Breed,
                IsNeedDigesting = afm.IsNeedDigesting,
                Description = afm.Description,
                Id = afm.Id
            };
        }
    }

    private async Task initContactAngle(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var ContactAngle =
            _contactAngleRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (ContactAngle is not null)
        {
            orderAnalyzeDto.ContactAngle = new ContactAngleAnalyzeDto()
            {
                Breed = ContactAngle.Breed,
                IsNeedDigesting = ContactAngle.IsNeedDigesting,
                Description = ContactAngle.Description,
                Id = ContactAngle.Id
            };
        }
    }

    private async Task initBet(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var existBet = _betRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (existBet is not null)
        {
            orderAnalyzeDto.Bet = new BETAnalyzeDto()
            {
                Name = orderAnalyze.Name,
                Time = existBet.Time,
                DegassingTemperature = existBet.DegassingTemperature,
                Id = existBet.Id
            };
        }
    }

    private async Task initTGA(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var exist = _tgaRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (exist is not null)
        {
            orderAnalyzeDto.TGA = new TGAAnalyzeDto()
            {
                TempertureStart = exist.TempertureStart,
                TempertureEnd = exist.TempertureEnd,
                Environment = exist.Environment,
                Rate = exist.Rate,
                Id = exist.Id
            };
        }
    }

    private async Task initXRF(OrderAnalyzeCommand orderAnalyzeDto, OrderAnalyze orderAnalyze)
    {
        var exist = _xrfRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (exist is not null)
        {
            orderAnalyzeDto.XRF = new XRFAnalyzeDto()
            {
                IsNeedGrind = exist.IsNeedGrind,
                IsNeedIOL = exist.IsNeedIOL,
                Type = exist.Type,
                Id = exist.Id
            };
        }
    }

    public AnalyzerDevice Find(string key)
    {
        return _repository.TableNoTracking.FirstOrDefault(x => x.Key.Equals(key));
    }


    private string GetBet(OrderAnalyze orderAnalyze)
    {
        var exist = _betRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        return $"دمای گاززدایی: {exist.DegassingTemperature} درجه سانتی‌گراد، زمان: {exist.Time} دقیقه";
    }

    private string GetFTIR(OrderAnalyze orderAnalyze)
    {
        var exist = _ftirRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        return
            $"حالت نمونه: {exist.SampleState}، نوع نمونه: {exist.SampleType}، ترکیب نمونه: {exist.SampleComposition}، ";
    }

    private string GetAAS(OrderAnalyze orderAnalyze)
    {
        var exist = _aasRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        return
            $"عنصر نمونه: {exist.Element}، نیاز به هضم دارد؟: {exist.IsNeedDigesting}، توضیحات نمونه: {exist.Description}";
    }

    private string GetAFM(OrderAnalyze orderAnalyze)
    {
        var exist = _afmRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        return
            $"جنس نمونه: {exist.Breed}، نیاز به هضم دارد؟: {exist.IsNeedDigesting}، توضیحات نمونه: {exist.Description}";
    }

    private string GetContactAngle(OrderAnalyze orderAnalyze)
    {
        var exist =
            _contactAngleRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        return
            $"جنس نمونه: {exist.Breed}، نیاز به هضم دارد؟: {exist.IsNeedDigesting}، توضیحات نمونه: {exist.Description}";
    }

    private string GetFireAssay(OrderAnalyze orderAnalyze)
    {
        var exist =
            _fireAssayRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        return
            $"عنصر نمونه: {exist.Element}، توضیحات نمونه: {exist.Description}";
    }

    private string GetEDX(OrderAnalyze orderAnalyze)
    {
        var exist = _edxRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        return
            $"عنصر نمونه: {exist.Element}";
    }

    private string GetMapping(OrderAnalyze orderAnalyze)
    {
        var exist = _mappingRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        return
            $"عنصر نمونه: {exist.Element}";
    }

    private string GetGCMS(OrderAnalyze orderAnalyze)
    {
        var exist = _gcmsRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        var composition = string.IsNullOrWhiteSpace(exist.Composition) ? "نامشخص" : exist.Composition;
        var solvent = string.IsNullOrWhiteSpace(exist.Solvent) ? "نامشخص" : exist.Solvent;
        var nature = string.IsNullOrWhiteSpace(exist.SampleNature) ? "نامشخص" : exist.SampleNature;
        return $"حلال: {solvent}، ماهیت: {nature}، ترکیبات: {composition}";
    }

    private string GetICPOES(OrderAnalyze orderAnalyze)
    {
        var exist =
            _icpoesRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        return
            $"عنصر نمونه: {exist.Element}، توضیحات نمونه: {exist.Description}";
    }

    private string GetSEM(OrderAnalyze orderAnalyze)
    {
        var exist = _semRepository.TableNoTracking
            .FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));

        if (exist == null)
            return "اطلاعاتی برای این نمونه یافت نشد.";

        var zoom = exist.Zoom.HasValue ? $"{exist.Zoom}" : "";
        var size = exist.Size.HasValue ? $"{exist.Size}" : "";
        var description = !string.IsNullOrWhiteSpace(exist.Description) ? exist.Description : "";

        return $" ، بزرگنمایی: {zoom}، اندازه: {size}، توضیحات نمونه: {description}";
    }

    private string GetTEM(OrderAnalyze orderAnalyze)
    {
        var exist = _temRepository.TableNoTracking
            .FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));

        if (exist == null)
            return "اطلاعاتی برای این نمونه یافت نشد.";

        var zoom = exist.Zoom.HasValue ? $"{exist.Zoom}" : "";
        var size = exist.Size.HasValue ? $"{exist.Size}" : "";
        var description = !string.IsNullOrWhiteSpace(exist.Description) ? exist.Description : "";

        return $" ، بزرگنمایی: {zoom}، اندازه: {size}، توضیحات نمونه: {description}";
    }

    private string GetTGA(OrderAnalyze orderAnalyze)
    {
        var exist = _tgaRepository.TableNoTracking
            .FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (exist == null)
            return "اطلاعاتی برای این نمونه یافت نشد.";
        var temperatureStart = exist.TempertureStart.HasValue ? $"{exist.TempertureStart} درجه سانتی‌گراد" : "نامشخص";
        var temperatureEnd = $"{exist.TempertureEnd}درجه سانتی‌گراد";
        var rate = $"{exist.Rate} درجه بر دقیقه";
        var environment = !string.IsNullOrWhiteSpace(exist.Environment) ? exist.Environment : "نامشخص";
        return
            $"دمای شروع: {temperatureStart}، دمای پایان: {temperatureEnd}، ریت: {rate}، محیط: {environment}";
    }

    private string GetUV(OrderAnalyze orderAnalyze)
    {
        var exist = _uvRepository.TableNoTracking
            .FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (exist is null)
            return "اطلاعاتی برای این نمونه یافت نشد.";
        var type = !string.IsNullOrWhiteSpace(exist.Type) ? exist.Type : "";
        var composition = !string.IsNullOrWhiteSpace(exist.Solvent) ? exist.Solvent : "";
        var waveLengthStart = exist.WaveLengthStart.HasValue ? $"{exist.WaveLengthStart} " : "";
        var waveLengthEnd = exist.WaveLengthEnd.HasValue ? $"{exist.WaveLengthEnd} " : "";
        var spectrum = !string.IsNullOrWhiteSpace(exist.Spectrum) ? exist.Spectrum : "";
        return
            $"ماهیت نمونه: {type}، حالل: {composition}، طول موج شروع: {waveLengthStart}، طول موج پایان: {waveLengthEnd}، طیف جذب: {spectrum}";
    }

    private string GetXRD(OrderAnalyze orderAnalyze)
    {
        var exist = _xrdRepository.TableNoTracking
            .FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (exist is null)
            return "اطلاعاتی برای این نمونه یافت نشد.";
        var type = !string.IsNullOrWhiteSpace(exist.Type) ? exist.Type : "";
        var composition = !string.IsNullOrWhiteSpace(exist.Composition) ? exist.Composition : "";
        var angleStart = $"{exist.AngleStart}درجه";
        var angleEnd = $"{exist.AngleEnd} درجه";
        return
            $"حالت نمونه: {type}، ترکیبات: {composition}، زاویه شروع: {angleStart}، زاویه خاتمه: {angleEnd}";
    }

    private string GetXRF(OrderAnalyze orderAnalyze)
    {
        var exist = _xrfRepository.TableNoTracking
            .FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        if (exist is null)
            return "اطلاعاتی برای این نمونه یافت نشد.";
        var type = !string.IsNullOrWhiteSpace(exist.Type) ? exist.Type : "";
        var isNeedIOL = exist.IsNeedIOL is true ? "نیاز به IOL دارد" : "";
        var isNeedGrind = exist.IsNeedGrind is true ? "خردایش و نرمایش نیاز دارد" : "";
        return
            $"حالت نمونه: {type}، {isNeedGrind}، {isNeedIOL}";
    }

    private string GetICPMS(OrderAnalyze orderAnalyze)
    {
        var exist =
            _icpmsRepository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(orderAnalyze.AnalyzeModelId));
        return
            $"عنصر نمونه: {exist.Element}، توضیحات نمونه: {exist.Description}";
    }

    public string FindKeyById(long Id)
    {
        var exist = _repository.TableNoTracking.FirstOrDefault(x => x.Id.Equals(Id));
        return exist?.Key;
    }
}