using System;
using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.TodoItems.Queries.GetPersonWithPagination;
using Common.Exceptions;
using Data.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Entities.Device;

public record class CreateAnalyzeDeviceAttributeDetailCommand : BaseRecordDto<CreateAnalyzeDeviceAttributeDetailCommand, AnalyzeDeviceAttributeDetail, int>, IRequest<int>
{
    public int AnalyzeAttributeId { get; set; }
    [MaxLength(100)]
    public string Title { get; set; }
    [MaxLength(100)]
    public string Description { get; set; }
}

public class CreateAnalyzeDeviceAttributeDetailCommandHandler : IRequestHandler<CreateAnalyzeDeviceAttributeDetailCommand, int>
{
    private readonly IRepository<AnalyzeDeviceAttributeDetail> _repository;
    private readonly IMapper _mapper;

    public CreateAnalyzeDeviceAttributeDetailCommandHandler(IRepository<AnalyzeDeviceAttributeDetail> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateAnalyzeDeviceAttributeDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}

