using System;
namespace CleanArchitecture.Infrastructure.Services.SMSProvider
{
    public class SendSMS
    {
        public string Message { get; set; }
        public string ToMobileNumber { get; set; }
        public string Code { get; set; }
    }
}