using System;
using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.Domain.Entities.BasicInformation;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Domain.Entities
{
    public class Person : BaseAuditableEntity<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [AllowNull] public string FatherName { get; set; }
        public string NationalId { get; set; }
        public string CompanyName { get; set; }
        public DateTime? Birthday { get; set; }
        public int? GenderTypeId { get; set; }
        public int? HomeTownCityId { get; set; }
        public int? CountryId { get; set; }

        public long? ImageId { get; set; }

        //public int? MaritalStatusId { get; set; }
        //public int? MilitaryStatusId { get; set; }
        //public int? ReligionTypesId { get; set; }
        public Country Nationality { get; set; }
        public Image Image { get; set; }
        public GenderType GenderType { get; set; }
        public City HomeTownCity { get; set; }
        public ICollection<PersonMobileNumber> MobileNumbers { get; set; }
        public ICollection<PeopleAddress> PeopleAddresses { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<DiscountUsed> DiscountUseds { get; set; }
        public ICollection<PersonDiscount> Discounts { get; set; }

        public ICollection<Orders.Order> Orders { get; set; }
        //public MaritalStatusType MaritalStatus { get; set; }
        //public MilitaryStatusType MilitaryStatus { get; set; }
        //public ReligionsType Religion { get; set; }
    }
}