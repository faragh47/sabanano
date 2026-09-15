using CleanArchitecture.Application.Orders.Commands.Create;
using CleanArchitecture.Application.PagesDto;
using NanoSaba.Utility;

namespace NanoSaba.Analyzes;

public class AnalyzeConverter
{
    private readonly HttpRequest _httpRequest;

    public AnalyzeConverter(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    public void GetAnalyzeByKey(in OrderAnalyzeCommand command, string key)
    {
        if (key.Equals("BET"))
        {
            command.Bet = GetBet();
        }
        else if (key.Equals("FTIR"))
        {
            command.Ftir = GetFTIR();
        }
        else if (key.Equals("AAS"))
        {
            command.AAS = GetAAS();
        }
        else if (key.Equals("AFM"))
        {
            command.AFM = GetAFM();
        }
        else if (key.Equals("ContactAngle"))
        {
            command.ContactAngle = GetContactAngle();
        }
        else if (key.Equals("EDX"))
        {
            command.EDX = GetEDX();
        }
        else if (key.Equals("Mapping"))
        {
            command.Mapping = GetMapping();
        }
        else if (key.Equals("FireAssay"))
        {
            command.FireAssay = GetFireAssay();
        }
        else if (key.Equals("GC-MS"))
        {
            command.GCMS = GetGCMS();
        }
        else if (key.Equals("ICP-MS"))
        {
            command.ICPMS = GetICPMS();
        }
        else if (key.Equals("ICP-OES"))
        {
            command.ICPOES = GetICPOES();
        }
        else if (key.Equals("SEM"))
        {
            command.SEM = GetSEM();
        }
        else if (key.Equals("TEM"))
        {
            command.TEM = GetTEM();
        }
        else if (key.Equals("TGA"))
        {
            command.TGA = GetTGA();
        }
        else if (key.Equals("UV-Vis"))
        {
            command.UV = GetUV();
        }
        else if (key.Equals("XRD"))
        {
            command.XRD = GetXRD();
        }
        else if (key.Equals("XRF"))
        {
            command.XRF = GetXRF();
        }
    }

    private BETAnalyzeDto GetBet()
    {
        BETAnalyzeDto bet = new();
        bet.DegassingTemperature = FormAttributesUtility.GetDoubleByKey("DegassingTemperature", _httpRequest);
        bet.Time = FormAttributesUtility.GetDoubleByKey("time", _httpRequest);
        return bet;
    }

    private FTIRAnalyzeDto GetFTIR()
    {
        FTIRAnalyzeDto ftir = new();
        ftir.SampleState = _httpRequest.Form["sampleState"].ToString();
        ftir.SampleType = _httpRequest.Form["sampleType"].ToString();
        ftir.SampleComposition = _httpRequest.Form["sampleComposition"].ToString();
        return ftir;
    }

    private AASAnalyzeDto GetAAS()
    {
        AASAnalyzeDto aas = new();
        aas.Element = _httpRequest.Form["element"].ToString();
        aas.IsNeedDigesting = FormAttributesUtility.GetBoolByKey("isNeedDigesting", _httpRequest);
        aas.Description = _httpRequest.Form["description"].ToString();
        return aas;
    }

    private AFMAnalyzeDto GetAFM()
    {
        AFMAnalyzeDto afm = new();
        afm.Breed = _httpRequest.Form["breed"].ToString();
        afm.IsNeedDigesting = FormAttributesUtility.GetBoolByKey("isNeedDigesting", _httpRequest);
        afm.Description = _httpRequest.Form["description"].ToString();
        return afm;
    }

    private ContactAngleAnalyzeDto GetContactAngle()
    {
        ContactAngleAnalyzeDto model = new();
        model.Breed = _httpRequest.Form["breed"].ToString();
        model.IsNeedDigesting = FormAttributesUtility.GetBoolByKey("isNeedDigesting", _httpRequest);
        model.Description = _httpRequest.Form["description"].ToString();
        return model;
    }

    private EDXAnalyzeDto GetEDX()
    {
        EDXAnalyzeDto model = new();
        model.Element = _httpRequest.Form["element"].ToString();
        return model;
    }

    private MappingAnalyzeDto GetMapping()
    {
        MappingAnalyzeDto model = new();
        model.Element = _httpRequest.Form["element"].ToString();
        return model;
    }

    private FireAssayAnalyzeDto GetFireAssay()
    {
        FireAssayAnalyzeDto model = new();
        model.Element = _httpRequest.Form["element"].ToString();
        model.Description = _httpRequest.Form["description"].ToString();
        return model;
    }

    private GCMSnalyzeDto GetGCMS()
    {
        GCMSnalyzeDto model = new();
        model.SampleNature = _httpRequest.Form["sampleNature"].ToString();
        model.Solvent = _httpRequest.Form["solvent"].ToString();
        model.Composition = _httpRequest.Form["composition"].ToString();
        return model;
    }

    private ICPMSAnalyzeDto GetICPMS()
    {
        ICPMSAnalyzeDto model = new();
        model.Element = _httpRequest.Form["element"].ToString();
        model.Description = _httpRequest.Form["description"].ToString();
        return model;
    }

    private ICPOESAnalyzeDto GetICPOES()
    {
        ICPOESAnalyzeDto model = new();
        model.Element = _httpRequest.Form["element"].ToString();
        model.Description = _httpRequest.Form["description"].ToString();
        return model;
    }

    private SEMAnalyzeDto GetSEM()
    {
        SEMAnalyzeDto model = new();
        model.Zoom = FormAttributesUtility.GetDoubleByKey("zoom", _httpRequest);
        model.Size = FormAttributesUtility.GetDoubleByKey("size", _httpRequest);
        model.Description = _httpRequest.Form["description"].ToString();
        return model;
    }

    private TEMAnalyzeDto GetTEM()
    {
        TEMAnalyzeDto model = new();
        model.Zoom = FormAttributesUtility.GetDoubleByKey("zoom", _httpRequest);
        model.Size = FormAttributesUtility.GetDoubleByKey("size", _httpRequest);
        model.Description = _httpRequest.Form["description"].ToString();
        return model;
    }

    private TGAAnalyzeDto GetTGA()
    {
        TGAAnalyzeDto model = new();
        model.TempertureStart = FormAttributesUtility.GetDoubleByKey("tempertureStart", _httpRequest);
        model.TempertureEnd = FormAttributesUtility.GetDoubleByKey("tempertureEnd", _httpRequest);
        model.Rate = FormAttributesUtility.GetDoubleByKey("rate", _httpRequest);
        model.Environment = _httpRequest.Form["environment"].ToString();
        return model;
    }

    private UVAnalyzeDto GetUV()
    {
        UVAnalyzeDto model = new();
        model.WaveLengthStart = FormAttributesUtility.GetDoubleByKey("waveLengthStart", _httpRequest);
        model.WaveLengthEnd = FormAttributesUtility.GetDoubleByKey("waveLengthEnd", _httpRequest);
        model.Solvent = _httpRequest.Form["solvent"].ToString();
        model.Type = _httpRequest.Form["type"].ToString();
        model.Spectrum = _httpRequest.Form["spectrum"].ToString();
        return model;
    }

    private XRDAnalyzeDto GetXRD()
    {
        XRDAnalyzeDto model = new();
        model.AngleStart = FormAttributesUtility.GetDoubleByKey("angleStart", _httpRequest);
        model.AngleEnd = FormAttributesUtility.GetDoubleByKey("angleEnd", _httpRequest);
        model.Composition = _httpRequest.Form["composition"].ToString();
        model.Type = _httpRequest.Form["type"].ToString();
        model.IsNeedGrind = FormAttributesUtility.GetBoolByKey("isNeedGrind", _httpRequest);
        return model;
    }

    private XRFAnalyzeDto GetXRF()
    {
        XRFAnalyzeDto model = new();
        model.Type = _httpRequest.Form["type"].ToString();
        model.IsNeedGrind = FormAttributesUtility.GetBoolByKey("isNeedGrind", _httpRequest);
        model.IsNeedIOL = FormAttributesUtility.GetBoolByKey("isNeedIOL", _httpRequest);
        return model;
    }
}