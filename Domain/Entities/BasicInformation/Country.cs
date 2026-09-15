using System;
namespace CleanArchitecture.Domain.Entities.BasicInformation
{
    public class Country: BaseAuditableEntity<int>
    {
        public string Title { get; set; }
        public string LatinName { get; set; }
        public ICollection<Province> Provinces { get; set; }
        public ICollection<Person> People { get; set; }
    }
}

