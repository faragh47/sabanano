using System;
namespace CleanArchitecture.Domain.Entities.HrManagment
{
    public class Polygon : BaseAuditableEntity<long>
    {
        public string Title { get; set; }
        public string Comments { get; set; }
        public string FeatureType { get; set; }
        public string FeatureJson { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public long? AddressId { get; set; }
        public Address Address { get; set; }
    }
}

