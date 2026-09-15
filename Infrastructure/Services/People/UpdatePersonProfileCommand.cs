using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.HrManagment.People.Commands.CreatePersonItem;
using CleanArchitecture.Application.People.Commands.UpdateAddress;
using CleanArchitecture.Application.People.Commands.UpdatePeopleAddress;
using CleanArchitecture.Application.People.Commands.UpdatePersonMobileNumber;
using CleanArchitecture.Application.TodoItems.Commands.CreateTodoItem;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.FinancialAggregate;
using CleanArchitecture.Infrastructure.Identity;
using Common.Exceptions;
using Data.Contracts;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.IServices.V2;

namespace CleanArchitecture.Application.People.Commands.UpdatePerson;

public record UpdatePersonProfileCommand : BaseRecordDto<UpdatePersonProfileCommand, Person, long>, IRequest<long>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyName { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Password { get; set; }

    public override void CustomMappings(IMappingExpression<UpdatePersonProfileCommand, Person> mapping)
    {
        mapping.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdatePersonProfileCommand, long>
{
    private readonly IRepository<Person> _repository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;
    private readonly IUsersService _usersService;

    public UpdateTodoItemCommandHandler(IRepository<Person> repository,
        IRepository<PersonMobileNumber> mobileNumberRepository
        , IMediator mediator, IUsersService usersService, UserManager<ApplicationUser> userManager = null)

    {
        _repository = repository;
        _mediator = mediator;
        _usersService = usersService;
        _userManager = userManager;
    }

    public async Task<long> Handle(UpdatePersonProfileCommand request, CancellationToken cancellationToken)
    {
        var person = await _repository
            .TableNoTracking
            .Include(x => x.Customers)
            .Include(x => x.PeopleAddresses)
            .Include(x => x.MobileNumbers)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (person is null)
            throw new BadRequestException("شناسه فرد اشتباه است");

        var updatePerson = await _mediator.Send(new UpdatePersonCommand()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,

            Id = person.Id,
            MobileNumbers = new List<UpdatePersonMobileNumberCommand>()
            {
                new UpdatePersonMobileNumberCommand()
                {
                    Id = person.MobileNumbers.FirstOrDefault().Id,
                    MobileNumber = request.MobileNumber,
                    IsDefault = true,
                    PersonId = person.Id
                }
            },
        });

        if (person.PeopleAddresses is { Count: > 0 })
        {
            var updateAddress = await _mediator.Send(new UpdateAddressCommand()
            {
                FullAddress = request.Address,
                Id = person.PeopleAddresses.FirstOrDefault().AddressId
            });
        }
        else
        {
            var createAddress = await _mediator.Send(new CreatePeopleAddressCommand()
            {
                PersonId = request.Id,
                Title = request.Address,
                Address = new CreateAddressCommand()
                {
                FullAddress = request.Address
            }
            });
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PersonId == person.Id);

        if (!string.IsNullOrEmpty(request.Email))
        {
            if (user is not null)
            {
                user.Email = request.Email;
                user.FullName = $"{request.FirstName} {request.LastName}";
                await _userManager.UpdateAsync(user);
            }
        }

        if (!string.IsNullOrEmpty(request.Password))
        {
            await _usersService.ChangePassword(new ApplicationUserSearchDto.ApplicationUserChangePasswordDto()
            {
                NewPassword = request.Password,
                UserName = user.UserName
            }, true, cancellationToken);
        }

        return person.Id;
    }
}