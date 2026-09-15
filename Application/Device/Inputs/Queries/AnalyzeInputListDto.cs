using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.HrManagment;




public class AnalyzeDeviceInputListDto : BaseDto<AnalyzeDeviceInputListDto, AnalyzeDeviceInput,int>
{
    public string Title { get; set; }
    public int AnalyzeDeviceId { get; set; }
    public bool? isList { get; set; }
    public string TextBox { get; set; }
    public bool? IsCheckbox { get; set; }
}


public class AnalyzeDeviceInputBriefListDto : BaseDto<AnalyzeDeviceInputBriefListDto, AnalyzeDeviceInput, int>
{
    public string Title { get; set; }
    public int AnalyzeDeviceId { get; set; }
    public bool? isList { get; set; }
    public string TextBox { get; set; }
    public bool? IsCheckbox { get; set; }
}
