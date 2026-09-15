using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.HrManagment.Polygon;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.People.Commands.UpdatePolygon;

public record UpdatePolygonCommand : BaseRecordDto<UpdatePolygonCommand, Polygon, long>, IRequest<long>
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
    public override void CustomMappings(IMappingExpression<UpdatePolygonCommand, Polygon> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdatePolygonCommand,long>
{
    private readonly IRepository<Polygon> _repository;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IRepository<Polygon> repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public record Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public async Task<long> Handle(UpdatePolygonCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.TableNoTracking.FirstAsync(x => x.Id == request.Id);

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

        request.MaxX = points.Select(x => x.X).Max();
        request.MinX = points.Select(x => x.X).Min();
        request.MaxY = points.Select(x => x.Y).Max();
        request.MinY = points.Select(x => x.Y).Min();
        request.CenterX = (request.MaxX + request.MinX) / 2;
        request.CenterY = (request.MaxY + request.MinY) / 2;

        var Polygon= request.ToEntity(_mapper, entity);

        await _repository.UpdateAsync(entity,cancellationToken);

        return entity.Id;
    }

   
}
