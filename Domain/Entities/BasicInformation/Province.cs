using System;
namespace CleanArchitecture.Domain.Entities.BasicInformation
{
    public class Province: BaseAuditableEntity<int>
    {
        public string Title { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}

