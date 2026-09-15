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
using CleanArchitecture.Domain.Entities.Email;

public record class CreateEmailDiscountCommand : BaseRecordDto<CreateEmailDiscountCommand, EmailDiscount, int>, IRequest<int>
{
    [MaxLength(100)]
    public string Email { get; set; }
}

public class CreateEmailDiscountCommandHandler : IRequestHandler<CreateEmailDiscountCommand, int>
{
    private readonly IRepository<EmailDiscount> _repository;
    private readonly IMapper _mapper;

    public CreateEmailDiscountCommandHandler(IRepository<EmailDiscount> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateEmailDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity(_mapper);

        await _repository.AddAsync(entity, cancellationToken);

        return entity.Id;
    }
}

