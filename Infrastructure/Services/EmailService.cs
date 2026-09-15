using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Common;
using DataTransferObjects.SharedModels;
using Common;
using CleanArchitecture.Application.Common.Interfaces;
using System.Net.Http;
using CleanArchitecture.Application.Common;

namespace CleanArchitecture.Infrastructure.Services;
public class EmailService : IEmailService
{
    public async Task<ApiResult> SendEmail(Email email)
    {
        try
        {
            MailMessage message = new MailMessage();

            message.From = new MailAddress(email.From);
            message.To.Add(new MailAddress(email.To));
            message.Subject = email.Subject;
            message.Body = email.Body;
            message.IsBodyHtml = true;
            /*message.From = new MailAddress("socket1socket@gmail.com");
            message.To.Add(new MailAddress("socket02socket@gmail.com"));
            message.Subject = "Test";
            message.Body = "Content";*/

            using (SmtpClient smtp = new SmtpClient("webmail.iranfoodguide.com", 587))
            //using (SmtpClient smtp = new SmtpClient("webmail.iranfoodguide.com", 110))
            {
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(email.From, email.Password);
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }



            return new ApiResult(true, ApiResultStatusCode.Success, null);
        }
        catch (Exception ex)
        {
            return new ApiResult(false, ApiResultStatusCode.LogicError, null);
        }
    }
}
