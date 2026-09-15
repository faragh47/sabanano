using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.HrManagment.Polygon;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Domain.Events;
using Common.Utilities;
using Data.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;

public record CreatePolygonCommand : BaseRecordDto<CreatePolygonCommand, Polygon, long>, IRequest<long>
{
    public long? AddressId { get; set; }
    public string Title { get; set; }
    public string Comments { get; set; }
    public string FeatureType { get; set; }
    [JsonProperty("FeatureJson")]
    public string FeatureJson { get; set; }
    public double CenterX { get; set; }
    public double CenterY { get; set; }
    public double MinX { get; set; }
    public double MinY { get; set; }
    public double MaxX { get; set; }
    public double MaxY { get; set; }
}

public class CreatePolygonCommandHandler : IRequestHandler<CreatePolygonCommand, long>
{
    private readonly IRepository<Polygon> _repository;
    private readonly IMapper _mapper;

    public CreatePolygonCommandHandler(IRepository<Polygon> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public record Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public async Task<long> Handle(CreatePolygonCommand request, CancellationToken cancellationToken)
    {
        //string json = "{\"type\":\"Feature\",\"id\":358,\"geometry\":{\"type\":\"Polygon\",\"coordinates\":[[[52.372835300000077,35.232541100000049,0],[52.367170500000043,35.235205200000053,0],[52.359960700000045,35.23758870000006,0],[52.34708610000007,35.241935000000069,0],[52.335241500000052,35.241234000000077,0],[52.319620300000054,35.242776200000037,0],[52.311380500000041,35.239972200000068,0],[52.303140800000051,35.235064900000054,0],[52.29936420000007,35.226932300000044,0],[52.29936420000007,35.220341400000052,0],[52.298505900000066,35.21459150000004,0],[52.300050900000031,35.209402200000056,0],[52.304170800000065,35.203932000000066,0],[52.317388700000038,35.198180900000068,0],[52.333524900000043,35.195655900000077,0],[52.349317700000029,35.195936500000073,0],[52.359960700000045,35.202108500000065,0],[52.363565600000072,35.207438600000046,0],[52.366827200000046,35.214451200000042,0],[52.368715400000042,35.222304700000052,0],[52.369917100000066,35.228194300000041,0],[52.372835300000077,35.232541100000049,0]]]},\"properties\":{\"OID\":358,\"Name\":\"گرمسار\",\"FolderPath\":\"نمایندگان تیپاکس - فعال/محدوده نمایندگی سمنان-مرکزی-ایلام-زنجان-قزوین\",\"SymbolID\":5,\"AltMode\":0,\"Base\":0,\"Clamped\":-1,\"Extruded\":0,\"Snippet\":\"\",\"PopupInfo\":\"\",\"Shape_Length\":0.19927063223232408,\"Shape_Area\":0.0027570395997993892,\"JetId\":\"F8FA75AB-1737-4EF9-A693-123EB3402CF8\"}}";

        //request.FeatureJson = json;

        var resultJson = JsonConvert.DeserializeObject<PolygonJsonDto>(request.FeatureJson);

        List<Point> points = new List<Point>();

        foreach (var item in resultJson.Geometry.Coordinates[0])
        {
            points.Add(new Point()
            {
                X = item[0],
                Y = item[1]
            });
        }

        request.MaxX= points.Select(x=>x.X).Max();
        request.MinX= points.Select(x=>x.X).Min();
        request.MaxY= points.Select(x=>x.Y).Max();
        request.MinY= points.Select(x=>x.Y).Min();
        request.CenterX = (request.MaxX + request.MinX)/2;
        request.CenterY= (request.MaxY + request.MinY) / 2;

        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        // entity.AddDomainEvent(new PolygonCreatedEvent(entity));

        //_context.TodoItems.Add(entity);

        return entity.Id;
    }
}
