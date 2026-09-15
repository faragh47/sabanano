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

public record class CreateAnalyzeDeviceServiceCommand : BaseRecordDto<CreateAnalyzeDeviceServiceCommand, AnalyzeDeviceService, int>, IRequest<int>
{
    public int AnalyzeDeviceId { get; set; }
    [MaxLength(100)]
    public string Title { get; set; }
    [MaxLength(100)]
    public string Description { get; set; }
    public decimal? Price { get; set; }
}

public class CreateAnalyzeDeviceServiceCommandHandler : IRequestHandler<CreateAnalyzeDeviceServiceCommand, int>
{
    private readonly IRepository<AnalyzeDeviceService> _repository;
    private readonly IMapper _mapper;

    public CreateAnalyzeDeviceServiceCommandHandler(IRepository<AnalyzeDeviceService> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateAnalyzeDeviceServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}

