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

public record class CreateAnalyzeDeviceAttributeCommand : BaseRecordDto<CreateAnalyzeDeviceAttributeCommand, AnalyzeDeviceAttribute, int>, IRequest<int>
{
    public int SampleCategoryId { get; set; }
    public int AnalyerDeviceId { get; set; }
}

public class CreateAnalyzeDeviceAttributeCommandHandler : IRequestHandler<CreateAnalyzeDeviceAttributeCommand, int>
{
    private readonly IRepository<AnalyzeDeviceAttribute> _repository;
    private readonly IMapper _mapper;

    public CreateAnalyzeDeviceAttributeCommandHandler(IRepository<AnalyzeDeviceAttribute> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateAnalyzeDeviceAttributeCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}

