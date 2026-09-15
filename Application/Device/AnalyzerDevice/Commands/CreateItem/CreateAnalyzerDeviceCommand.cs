using System;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Entities.Device;
using System.ComponentModel.DataAnnotations;

public record class CreateAnalyzerDeviceCommand : BaseRecordDto<CreateAnalyzerDeviceCommand, AnalyzerDevice, int>, IRequest<int>
{
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(100)]
    public string PersianName { get; set; }
    [MaxLength(100)]
    public string FullName { get; set; }
    [MaxLength(100)]
    public string Country { get; set; }
    [MaxLength(100)]
    public string CompanyName { get; set; }
    [MaxLength(100)]
    public string SpectroscopyRange { get; set; }
    [MaxLength(450)]
    public string Usage { get; set; }
    [MaxLength(100)]
    public string Model { get; set; }
    [MaxLength(450)]
    public string MaintenanceCondition { get; set; }
    public decimal Price { get; set; }
    public int? DiscountPercent { get; set; }
    [MaxLength(450)]
    public string DescriptionForReadyAnalyze { get; set; }
    public List<CreateAnalyzerDeviceSampleCommand> Samples { get; set; }
    public List<CreateAnalyzeDeviceServiceCommand> Services { get; set; }
    public List<CreateAnalyzeDeviceAttributeCommand> Attributes { get; set; }
    public List<CreateAnalyzeDeviceInputCommand> Inputs { get; set; }
}

public class CreateAnalyzerDeviceCommandHandler : IRequestHandler<CreateAnalyzerDeviceCommand, int>
{
    private readonly IRepository<AnalyzerDevice> _repository;
    private readonly IMapper _mapper;

    public CreateAnalyzerDeviceCommandHandler(IRepository<AnalyzerDevice> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateAnalyzerDeviceCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        return entity.Id;
    }
}

