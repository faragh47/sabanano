using System;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Infrastructure.Services.SMSProvider.Faraz;
using Common;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using DataTransferObjects.DataTransferObjects.UserDTOs;
using DataTransferObjects.SharedModels;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;

namespace CleanArchitecture.Infrastructure.Services
{
    public class UserCodeService : IUserCodeService
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IRepository<AccUserCode> _userCodeRepository;
        private readonly IFarazSMSProvider _farazSMSProvider;
        private readonly IJwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public UserCodeService(IRepository<ApplicationUser> userRepository, IRepository<AccUserCode> userCodeRepository,
            IHttpClientFactory httpClientFactory, IFarazSMSProvider farazSMSProvider, IJwtService jwtService,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userRepository = userRepository;
            _userCodeRepository = userCodeRepository;
            _farazSMSProvider = farazSMSProvider;
            _jwtService = jwtService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApiResult<RegisterDto>> GenerateCode(string MobileNumber, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(MobileNumber))
                return new ApiResult<RegisterDto>(false, ApiResultStatusCode.AtleastOneMobileNoRequired, null);

            var resultUser = await _userRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Person.MobileNumbers.Any(x => x.MobileNumber == MobileNumber));

            if (resultUser is null)
                return new ApiResult<RegisterDto>(true, ApiResultStatusCode.NotFound, new RegisterDto()
                {
                    IsRegistered = false
                });

            var roles = await _userManager.GetRolesAsync(resultUser);
            var requestsCount = await _userCodeRepository.TableNoTracking
                .Where(x => x.UserId == resultUser.Id && x.Created > DateTime.Now.AddHours(-24)).CountAsync();

            var lastRequestCode = await _userCodeRepository.TableNoTracking.Where(x => x.UserId == resultUser.Id)
                .OrderByDescending(x => x.Created).FirstOrDefaultAsync();

            //if (requestsCount >= 3 || lastRequestCode?.Created > DateTime.Now.AddMinutes(1))
            //    return new ApiResult<RegisterDto>(false, ApiResultStatusCode.ToManyRequestAllowedInForgetPassword, null);

            var code = CodeGenerator.GenerateNumberCode(4);

            var entity = new AccUserCode()
            {
                Code = code,
                UserId = (long)resultUser.Id,
            };

            await _userCodeRepository.AddAsync(entity, cancellationToken);

            if (MobileNumber.StartsWith("0"))
            {
                MobileNumber = MobileNumber.Substring(1, MobileNumber.Length - 1);
                MobileNumber = "+98" + MobileNumber;
            }

            if (!roles.Any(x => x.Equals("SuperAdmin")))
                await _farazSMSProvider.SendPatternSms(new SMSProvider.SendSMS
                {
                    ToMobileNumber = MobileNumber,
                    Code = code
                });

            return new ApiResult<RegisterDto>(true, ApiResultStatusCode.Success,
                new RegisterDto() { IsRegistered = true });
        }

        public async Task<ApiResult<AccessToken>> Token(CodeWithMobileNumber dto, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(dto.MobileNumber))
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.AtleastOneMobileNoRequired, null);

            if (String.IsNullOrEmpty(dto.Code))
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.BadRequest, null);

            var resultUser = await _userRepository.TableNoTracking.FirstOrDefaultAsync(x =>
                x.Person.MobileNumbers.Any(x => x.MobileNumber == dto.MobileNumber));

            if (resultUser is null)
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.NotFound, null);

            var lastRequestCode = await _userCodeRepository.TableNoTracking
                .Where(x => x.UserId == resultUser.Id && x.IsActive == true)
                .OrderByDescending(x => x.Created).FirstOrDefaultAsync();
            var roles = await _userManager.GetRolesAsync(resultUser);
            if (!roles.Any(x => x.Equals("SuperAdmin")) && (lastRequestCode.Code != dto.Code ||
                                                       lastRequestCode.Created < (DateTime.Now.AddMinutes(-2))))
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.ForgetPasswordCodeIsNotValid, null);
            var jwt = await _jwtService.GenerateAsync(resultUser);
            jwt.roles = roles;
            jwt.PersonId = resultUser.PersonId;
            var userIdentity = await _signInManager.CreateUserPrincipalAsync(resultUser);
            jwt.Identity = userIdentity;
            //Update last login
            resultUser.LastLoginDate = DateTime.Now;
            await _userRepository.UpdateAsync(resultUser, cancellationToken);

            var requests = await _userCodeRepository.TableNoTracking.Where(x => x.UserId == resultUser.Id)
                .ToListAsync();

            foreach (var item in requests)
            {
                await _userCodeRepository.DeActiveAsync(item, cancellationToken);
            }

            return jwt;
        }
    }
}