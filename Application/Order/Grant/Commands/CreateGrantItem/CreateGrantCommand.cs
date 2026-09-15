using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Grants;
using CleanArchitecture.Domain.Entities.HrManagment.Companies;
using CleanArchitecture.Domain.Entities.Order;
using Common.Utilities;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.HrManagment;

public record CreateGrantCommand : BaseRecordDto<CreateGrantCommand, Grant, long>, IRequest<long>
{
    public string NationalCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UniversityName { get; set; }
    public string TelNumber { get; set; }
}

public class CreateGrantCommandHandler : IRequestHandler<CreateGrantCommand, long>
{
    private readonly IRepository<Grant> _repository;
    private readonly IMapper _mapper;

    public CreateGrantCommandHandler(IRepository<Grant> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateGrantCommand request, CancellationToken cancellationToken)
    { var entity = request.ToEntity(_mapper);
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}