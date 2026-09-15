using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.HrManagment;
using CleanArchitecture.Infrastructure.Common;
using CleanArchitecture.Infrastructure.Identity;
using Common;
using Data.Contracts;
using DataTransferObjects.SharedModels;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Services;
public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IEmailService _emailService;
    private readonly IRepository<ApplicationUser> _userRepository;

    public CustomerService(IRepository<Customer> customerRepository,
        IEmailService emailService,
        IRepository<ApplicationUser> userRepository)
    {
        _customerRepository = customerRepository;
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task<ApiResult> SendEmailToCustomer(long CustomerId, Email email)
    {
        var customer = await _customerRepository.TableNoTracking.Where(x => x.Id == CustomerId).FirstOrDefaultAsync();

        if (customer is null)
            return new ApiResult(false, ApiResultStatusCode.NotFound, null);
        var user = await _userRepository.TableNoTracking.FirstOrDefaultAsync(x => x.PersonId == customer.PersonId);

        if (user is null)
            return new ApiResult(false, ApiResultStatusCode.NotFound, null);

        email.To = user.Email;

        var result=await _emailService.SendEmail(email);

        return result;
    }
}
