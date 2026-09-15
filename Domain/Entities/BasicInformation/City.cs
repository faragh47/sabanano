using System;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Domain.Entities.BasicInformation
{
    public class City:BaseAuditableEntity<int>
    {
        public string Title { get; set; }
        public int ProvinceId { get; set; }
        public string LatinName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Province Province { get; set; }
        public ICollection<Person> People { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}

