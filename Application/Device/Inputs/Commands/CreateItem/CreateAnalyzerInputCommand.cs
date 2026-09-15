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

public record class CreateAnalyzeDeviceInputCommand : BaseRecordDto<CreateAnalyzeDeviceInputCommand, AnalyzeDeviceInput, int>, IRequest<int>
{
    public string Title { get; set; }
    public int AnalyzeDeviceId { get; set; }
    public bool? isList { get; set; }
    public string TextBox { get; set; }
    public bool? IsCheckbox { get; set; }
}

public class CreateAnalyzeDeviceInputCommandHandler : IRequestHandler<CreateAnalyzeDeviceInputCommand, int>
{
    private readonly IRepository<AnalyzeDeviceInput> _repository;
    private readonly IMapper _mapper;

    public CreateAnalyzeDeviceInputCommandHandler(IRepository<AnalyzeDeviceInput> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateAnalyzeDeviceInputCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}

