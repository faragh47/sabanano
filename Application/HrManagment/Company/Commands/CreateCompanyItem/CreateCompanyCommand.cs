using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.HrManagment.Companies;
using Common.Utilities;
using Data.Contracts;
using MediatR;

namespace CleanArchitecture.Application.HrManagment;

public record CreateCompanyCommand : BaseRecordDto<CreateCompanyCommand, Company, long>, IRequest<long>
{
    public string NationalCode { get; set; }
    public string RegisterCode { get; set; }
    public string MobileNumber { get; set; }
    public string EconomicNumber { get; set; }
}

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, long>
{
    private readonly IRepository<Company> _repository;
    private readonly IMapper _mapper;

    public CreateCompanyCommandHandler(IRepository<Company> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}