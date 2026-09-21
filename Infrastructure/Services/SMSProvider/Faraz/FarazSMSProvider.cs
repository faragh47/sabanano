using DataTransferObjects.SharedModels;
using Services.IServices.OutgoingApi;
using System;
using System.Xml.Linq;

namespace CleanArchitecture.Infrastructure.Services.SMSProvider.Faraz
{
    public class FarazSMSProvider : ApiClientFactory, IFarazSMSProvider
    {
        public FarazSMSProvider(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }


        public async Task SendPatternSms(SendSMS sendSMS)
        {
            string url = "https://api.iranpayamak.com/ws/v1/sms/pattern";

            var recipient = sendSMS.ToMobileNumber.Trim().Replace(" ", "").Replace("-", "");
            if (recipient.StartsWith("+98"))
            {
                recipient = "0" + recipient[3..];
            }
            else if (recipient.StartsWith("98") && recipient.Length == 12)
            {
                recipient = "0" + recipient[2..];
            }

            var request = new SendPatternRequest
            {
                Code = "LzFNb9Z4LS",
                LineNumber = "50002178584000",
                Recipient = recipient,
                Attributes = new Dictionary<string, string>
                {
                    ["code"] = sendSMS.Code
                }
            };

            var result = await PostAsync(url, request, new HttpHeader()
            {
                Name = "Api-Key",
                Value = "vt6oOw24VPg7ojcvptI1TAHKszpkFplLQ0uLvyagPLsACy4npj"
            });

            var apiResult = await GenerateResultValueAsync<AccessTokenFaraz>(result);

            if (apiResult.Data is not null)
                AccessTokenFaraz = apiResult.Data;

        }
    }
}

