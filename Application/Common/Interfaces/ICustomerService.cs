using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common;
using Common;
using DataTransferObjects.SharedModels;

namespace CleanArchitecture.Application.Common.Interfaces;
public interface ICustomerService : IScopedDependency
{
    public Task<ApiResult> SendEmailToCustomer(long CustomerId, Email email);
}
