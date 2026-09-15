using CleanArchitecture.Infrastructure.Identity;
using Common;
using Data.Contracts;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //public AuthenticationMiddleware()
        //{
        //}

        //public async Task Invoke(HttpContext httpContext, SignInManager<ApplicationUser> signInManager)
        //{
        //    string token = httpContext.Request.Cookies["_token"];
        //    //if (signInManager.IsSignedIn(httpContext.User)){

        //    //    if (!string.IsNullOrEmpty(token))
        //    //    {
        //    //        await signInManager.SignOutAsync();
        //    //    }
        //    //}
        //    using var buffer = new MemoryStream();
        //    var request = httpContext.Request;
        //    var response = httpContext.Response;

        //    var stream = response.Body;
        //    response.Body = buffer;

        //    await _next(httpContext);

        //    Debug.WriteLine($"Request content type:  {httpContext.Request.Headers["Accept"]} {System.Environment.NewLine} Request path: {request.Path} {System.Environment.NewLine} Response type: {response.ContentType} {System.Environment.NewLine} Response length: {response.ContentLength ?? buffer.Length}");
        //    buffer.Position = 0;

        //    //await buffer.CopyToAsync(stream);
        //}
   
        public async Task Invoke(HttpContext httpContext,IJwtService jwtService ,IRepository<ApplicationUser> repository, SignInManager<ApplicationUser> signInManager)
        {
            string token = httpContext.Request.Cookies["_token"];
            var userId= jwtService.ValidateToken(token);
            if (userId is not null)
            {
                var existingUser = repository.TableNoTracking.FirstOrDefault(x => x.Id == userId);
                if (existingUser is not null)
                {
                    var userIdentity = await signInManager.CreateUserPrincipalAsync(existingUser);
                    var claimsIdentity = userIdentity.Identity as ClaimsIdentity;
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Actor ,existingUser.FullName));
                    httpContext.User = userIdentity;
                }
            }
            if (signInManager.IsSignedIn(httpContext.User))
            {

                if (!string.IsNullOrEmpty(token))
                {
                    await signInManager.SignOutAsync();
                }
            }

            //await signInManager.SignOutAsync();

            using var buffer = new MemoryStream();
            var request = httpContext.Request;
            var response = httpContext.Response;

            var stream = response.Body;
            response.Body = buffer;

            await _next(httpContext);

            //Debug.WriteLine($"Request content type:  {httpContext.Request.Headers["Accept"]} {System.Environment.NewLine} Request path: {request.Path} {System.Environment.NewLine} Response type: {response.ContentType} {System.Environment.NewLine} Response length: {response.ContentLength ?? buffer.Length}");
            buffer.Position = 0;

            await buffer.CopyToAsync(stream);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.  
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}

