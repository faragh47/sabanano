using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;

namespace CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;

public class PolygonDto : BaseDto<PolygonDto, Polygon,long>
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
}


public class PolygonBriefDto : BaseDto<PolygonBriefDto, Polygon, long>
{
    public string Title { get; set; }
    public string FeatureJson { get; set; }
    public double CenterX { get; set; }
    public double CenterY { get; set; }
    public double MinX { get; set; }
    public double MinY { get; set; }
    public double MaxX { get; set; }
    public double MaxY { get; set; }
}
