using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.Device;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Entities.Articles;

namespace CleanArchitecture.Domain.Entities;

public class Image : BaseAuditableEntity<long>
{
    public string Title { get; set; }
    public string LatinTitle { get; set; }
    public string FileName { get; set; }
    public string FileExt { get; set; }
    public long SizeInBytes { get; set; }
    public string SHA256 { get; set; }
    public ICollection<Person> People { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public ICollection<Orders.Order> Orders { get; set; }
    public ICollection<ArticleDetail> ArticleDetails { get; set; }
    public ICollection<Article> Articles { get; set; }
    public ICollection<AnalyzeDeviceResponse> AnalyzeDeviceResponses { get; set; }
    public ICollection<Complaint> Complaints { get; set; }
}