using System;
using CleanArchitecture.Domain.Entities.BasicInformation;

namespace CleanArchitecture.Domain.Entities.HrManagment
{
    public class Address : BaseAuditableEntity<long>
    {
        public int? CityId { get; set; }
        public string FullAddress { get; set; }
        public string? PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Floor { get; set; }
        public int? Unit { get; set; }
        public int? Number { get; set; }
        public City City { get; set; }
        public ICollection<PeopleAddress> PeopleAddresses { get; set; }
        public ICollection<Polygon> Polygons { get; set; }
    }
}