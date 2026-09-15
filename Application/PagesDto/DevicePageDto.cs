using CleanArchitecture.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.PagesDto
{
    public class DevicePageDto : BaseViewModel
    {
        public AnalyzerDeviceListDto AnalyzerDevice { get; set; }
        public List<ArticleDto> Articles { get; set; }
        public AnalyzePageDto Analyze { get; set; }
        public DevicePageDto()
        {
            Analyze = new();
        }
    }
}
