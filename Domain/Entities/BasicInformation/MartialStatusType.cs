using System;
namespace CleanArchitecture.Domain.Entities.BasicInformation
{
    public class MaritalStatusType
    {
        public string Title { get; set; }
        public string LatinTitle { get; set; }
        public ICollection<Person> People { get; set; }
    }
}

