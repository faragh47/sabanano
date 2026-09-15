using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Azure.Core;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.HrManagment.People.Commands.CreatePersonItem;
using CleanArchitecture.Application.People.Commands.UpdatePerson;
using CleanArchitecture.Application.People.Commands.UpdatePersonMobileNumber;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using Common.Utilities;
using Data.Contracts;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.IServices.V2;

public record CreatePersonProfileCommand : BaseRecordDto<CreatePersonProfileCommand, Person, long>, IRequest<long>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FatherName { get; set; }
    public string CompanyName { get; set; }
    public string Email { get; set; }
    public string NationalId { get; set; }
    public string Password { get; set; }
    public string Sheba { get; set; }
    public string Birthday { get; set; }
    public int? GenderTypeId { get; set; }
    public int? HomeTownCityId { get; set; }
    public int? CountryId { get; set; }
    public List<CreatePersonMobileNumberCommand> MobileNumbers { get; set; }
    public List<CreatePeopleAddressCommand> Addresses { get; set; }

    public override void CustomMappings(IMappingExpression<CreatePersonProfileCommand, Person> mappingExpression)
    {
        mappingExpression.ForMember(dest => dest.Birthday, conf => conf.MapFrom(src => src.Birthday.ToGregorianDate()));
    }
}

public class CreatePersonProfileCommandHandler : IRequestHandler<CreatePersonProfileCommand, long>
{
    private readonly IRepository<Person> _repository;
    private readonly IRepository<PersonMobileNumber> _personMobileRepository;
    private readonly IMapper _mapper;
    private readonly IUsersService _usersService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;

    public CreatePersonProfileCommandHandler(IRepository<Person> repository,
        IMapper mapper,
        IUsersService usersService,
        ICurrentUserService currentUserService,
        IMediator mediator,
        IRepository<PersonMobileNumber> personMobileRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _usersService = usersService;
        _currentUserService = currentUserService;
        _mediator = mediator;
        _personMobileRepository = personMobileRepository;
    }

    public async Task<long> Handle(CreatePersonProfileCommand request, CancellationToken cancellationToken)
    {
        var entity = await _mediator.Send(new CreatePersonCommand()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Birthday = request.Birthday,
            CountryId = request.CountryId,
            FatherName = request.FatherName,
            GenderTypeId = request.GenderTypeId,
            HomeTownCityId = request.HomeTownCityId,
            MobileNumbers = request.MobileNumbers,
            PeopleAddresses = request.Addresses,
            NationalId = request.NationalId,
            CompanyName = request.CompanyName
        });
        if (entity <= 0)
            return 0;
        var mobileNumber = request.MobileNumbers.FirstOrDefault().MobileNumber;
        var result = await _usersService.CreateUser(new ApplicationUserCuDto
        {
            PersonId = entity,
            Email = request.Email,
            IsActive = true,
            Password = request.Password,
            UserName = mobileNumber
        }, Convert.ToInt64(_currentUserService.UserId), cancellationToken);
        if (result.IsSuccess is false)
        {
            return 0;
        }

        var customer = await _mediator.Send(new CreateCustomerCommand()
        {
            PersonId = entity,
            CustomerCode = CodeGenerator.GenerateCode(" ", 6),
            Sheba = request.Sheba,
            Title = request.FirstName + " " + request.LastName
        });

        await _usersService.AddRoleToUser(result.Data.Id, 3, cancellationToken); //cusotomer
        return entity;
        // entity.AddDomainEvent(new PersonProfileCreatedEvent(entity));
        //_context.TodoItems.Add(entity);
    }
}