using Common;

namespace CleanArchitecture.Infrastructure.Services.SMSProvider.Faraz;

public interface IFarazSMSProvider:IScopedDependency
{
    Task SendPatternSms(SendSMS sendSMS);
}